using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.ViewModelBases;
using System.Windows;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Csla.Core;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddSongMultiLineTextEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddSongMultiLineTextEditViewModel : ViewModelBase<MultiLineTextEdit, MultiLineTextDto>
  {
    public AddSongMultiLineTextEditViewModel()
    {
      Languages = Services.Container.GetExportedValue<LanguageSelectorViewModel>();
      Languages.SelectedItemChanged += HandleLanguageChanged;
      if (!bool.Parse(ViewViewModelResources.ShowInstructions))
        InstructionsVisibility = Visibility.Collapsed;
      else
        InstructionsVisibility = Visibility.Visible;
    }

    private LanguageEdit _SongLanguage;
    public LanguageEdit SongLanguage
    {
      get { return _SongLanguage; }
      set
      {
        if (value != _SongLanguage)
        {
          _SongLanguage = value;
          NotifyOfPropertyChange(() => SongLanguage);
        }
      }
    }

    private void HandleLanguageChanged(object sender, EventArgs e)
    {
      if (sender != null)
        SongLanguage = ((LanguageEditViewModel)sender).Model;
    }

    private LanguageSelectorViewModel _Languages;
    public LanguageSelectorViewModel Languages
    {
      get { return _Languages; }
      set
      {
        if (value != _Languages)
        {
          _Languages = value;
          NotifyOfPropertyChange(() => Languages);
        }
      }
    }

    public string LabelLanguageText { get { return ViewViewModelResources.LabelAddSongLanguage; } }
    public string LabelSongText { get { return ViewViewModelResources.LabelAddSongSongText; } }
    public string LabelSongTitle { get { return ViewViewModelResources.LabelAddSongSongTitle; } }
    public string LabelSongAdditionalMetadata { get { return ViewViewModelResources.LabelSongAdditionalMetadata; } }

    public string InstructionsSelectLanguage { get { return ViewViewModelResources.InstructionsAddSongSelectLanguage; } }
    public string InstructionsEnterSongText { get { return ViewViewModelResources.InstructionsAddSongEnterSongText; } }
    public string InstructionsEnterSongTitle { get { return ViewViewModelResources.InstructionsAddSongEnterSongTitle; } }
    public string InstructionsEnterSongAdditionalMetadata 
    { 
      get { return ViewViewModelResources.InstructionsAddSongEnterSongAdditionalMetadata; } 
    }

    private Visibility _InstructionsVisibility;
    public Visibility InstructionsVisibility
    {
      get { return _InstructionsVisibility; }
      set
      {
        if (value != _InstructionsVisibility)
        {
          _InstructionsVisibility = value;
          NotifyOfPropertyChange(() => InstructionsVisibility);
        }
      }
    }

    private string _SongText;
    public string SongText
    {
      get { return _SongText; }
      set
      {
        if (value != _SongText)
        {
          _SongText = value;
          NotifyOfPropertyChange(() => SongText);
        }
      }
    }

    public override bool CanSave
    {
      get
      {
        if (SongText.Length < 2 ||
            SongLanguage == null)
          return false;
        else
          return true;
      }
    }
    public override void Save()
    {
      //TODO: PUT ALL SAVE SONG STUFF INTO ONE SAVER COMMAND OBJECT.  THIS OBJECT SHOULD TAKE THE ENTERED SONG INFO, PARSE THE SONG, AND SAVE ALL SUBPHRASES AS WE DO HERE.  THIS SHOULD *NOT* GO INTO THE SONGEDIT MODEL SAVE, AS THE SONG MODEL SHOULD NOT PARSE EVERY TIME IT IS SAVED.

      ///when we save the song lyrics, we want to...
      ///  1) Parse each text line into a subphrase, parse each subphrase into a word-subphrase
      ///     if subphrase is unique and not already in the database, save that subphrase
      ///  2) Create LineEdits from text lines and add these to the song model.
      ///  3) Save the song model itself.
      ///e.g. 
      ///  this is line one
      ///  this is line two
      ///  this is line three
      ///  subphrases: "this is line one" "this is line two" "this is line three"
      ///  word-subphrases: "this" "is" "line" "one" "two" "three"
      #region PARSE THE SONGTEXT INTO LINES AND ADD TO MODEL.LINES
      {
        

        //PARSE PHRASE INTO LINES/WORDS
        
        //var parentPhrase = e.Model;
        var songText = SongText;
        var languageText = SongLanguage.Text;

        var lineDelimiter = ViewViewModelResources.LineDelimiter;
        lineDelimiter = lineDelimiter.Replace("\\r", "\r");
        lineDelimiter = lineDelimiter.Replace("\\n", "\n");
        var lines = new List<string>(songText.Split(new string[] { lineDelimiter }, StringSplitOptions.RemoveEmptyEntries));

        var splitIntoWordsPattern = ViewViewModelResources.RegExSplitPatternWords;
        splitIntoWordsPattern = splitIntoWordsPattern.Replace(@"\\", @"\");
        var words = new List<string>(Regex.Split(songText, splitIntoWordsPattern));

        //I'M CHANGING THIS TO UTILIZE MY LINELIST.NEWLINELIST(INFOS) ALREADY IN PLACE.
        //SO AT THIS TIME, WE ONLY NEED TO SAVE THE WORDS THEMSELVES.  THE LINES WILL BE SAVED
        //IN FUTURE STEP IN THIS BLOCK.
        var allWords = new List<string>(words);
        //var allSubPhrases = new List<string>(lines);
        //allSubPhrases.AddRange(words);

        ////REMOVE DUPLICATES - this is no longer necessary as this is done in the remove command execution
        //allSubPhrases = allSubPhrases.Distinct().ToList();

        #region REMOVE ALL WORDS THAT ALREADY EXIST IN DB, SO THAT WE ARE LEFT WITH ONLY NEW WORDS

        //we don't want to add duplicate words to the DB, so check to see if any of these words already exist in DB.
        RemoveAlreadyExistingPhraseTextsCommand.BeginExecute(languageText, allWords, (s, r) =>
          {
            if (r.Error != null)
              throw r.Error;

            var allWordsNotAlreadyInDatabase = r.Object.PrunedPhraseTexts;

            //CREATE PHRASE FOR EACH NEW WORD
            PhraseList.NewPhraseList(new Business.Criteria.PhraseTextsCriteria(languageText, 
                                                                               allWordsNotAlreadyInDatabase), 
                                                                               (s2, r2) =>
            {
              if (r2.Error != null)
                throw r2.Error;

              var phraseListContainingAllWordsNotAlreadyInDatabase = r2.Object;

              #region SAVE ALL WORDS THAT ARE NEW (NOT ALREADY IN DATABASE)

              phraseListContainingAllWordsNotAlreadyInDatabase.BeginSave((s3, r3) =>
              {
                if (r3.Error != null)
                  throw r3.Error;

                phraseListContainingAllWordsNotAlreadyInDatabase = (PhraseList)r3.NewObject;
                //SO NOW, ALL OF OUR WORDS IN THE SONG THAT WERE NOT ALREADY IN THE DATABASE ARE INDEED NOW 
                //STORED IN THE DATABASE.

                #region CREATE LINEEDIT OBJECTS FOR EACH TEXT LINE AND ADD TO MODEL

                var lineInfoDictionary = new Dictionary<int, string>();
                for (int i = 0; i < lines.Count; i++)
                {
                  lineInfoDictionary.Add(i, lines[i]);
                }

                var criteria2 = new Business.Criteria.LineInfosCriteria(languageText, lineInfoDictionary);
                LineList.NewLineList(criteria2, (s5, r5) =>
                  {

                  });

                var criteria = new Business.Criteria.PhraseTextsCriteria(languageText, lines);
                PhraseList.NewPhraseList(criteria, (s4, r4) =>
                  {
                    if (r4.Error != null)
                      throw r4.Error;

                    var phraseEditLines = r4.Object;

                    //LineList.NewLineList(

                    #region SET SONG TITLE (IF NECESSARY)
                    //IF THE SONGTITLE IS EMPTY, THEN USE THE FIRST LINE OF THE SONG AS THE SONG TITLE
                    #endregion

                    #region SAVE THE ACTUAL SONG MODEL

                    //NOW WE CAN USE THE BASE SAVE
                    base.Save();

                    #endregion

                  });

                
                #endregion
              });

              #endregion
            });

          });
        #endregion


      }



    }
  }
}
