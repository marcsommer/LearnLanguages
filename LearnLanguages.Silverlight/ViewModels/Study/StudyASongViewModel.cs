using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Silverlight.Interfaces;
using Caliburn.Micro;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.DataAccess;
using System.Collections.Generic;
using LearnLanguages.Common.Interfaces;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  /// <summary>
  /// When studying a song, we need 3 things:
  /// 1) User's Native Language
  /// 2) A song to study.
  /// 3) A StudyPartner to do the studying.
  /// 
  /// This view model checks for #1, and shows a modal dialog if it can't find the native language.
  /// This view model's main function is #2, select the song to study.  Once it has that, it will pass 
  /// control over to the StudyPartner who is then free to navigate however it likes.
  /// </summary>
  [Export(typeof(StudyASongViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class StudyASongViewModel : Conductor<StudyASongItemViewModel>.Collection.AllActive,
                                     IHandle<Navigation.EventMessages.NavigatedEventMessage>,
                                     IViewModelBase
  {
    #region Ctors and Init

    public StudyASongViewModel()
    {
      //_AskViewModel = Services.Container.GetExportedValue<AskDoYouKnowThisViewModel>();
      if (_StudyPartner == null)
        Services.Container.SatisfyImportsOnce(this);

      if (_StudyPartner == null)
        throw new Common.Exceptions.PartNotSatisfiedException("StudyPartner");

      Services.EventAggregator.Subscribe(this);
      //THE NEXT INIT STUFF HAPPEN AS HANDLE(NAVIGATED MESSAGE)
    }
    
    private void InitializeViewModel()
    {
      //RATHER COMPLICATED BECAUSE OF ASYNC.  THE STEPS OF INITIALIZATION ARE:
      //1) GET NATIVE LANGUAGE
      //2) POPULATE SONGS

      //WE MUST FIRST KNOW WHAT THE USER'S NATIVE LANGUAGE IS.
      StudyDataRetriever.CreateNew((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        StudyData = r.Object.StudyData;

        if (!r.Object.StudyDataAlreadyExisted)
          AskUserForExtraDataAsync((s2, r2) =>
          {
            if (r2.Error != null)
              throw r2.Error;

            StudyData = r2.Object;
            SelectASong();
          });
        else
        {
          SelectASong();
        }
      });
    }

    #endregion

    #region Fields

    #endregion

    #region Properties

    private IStudyPartner _StudyPartner;
    [Import]
    public IStudyPartner StudyPartner
    {
      get { return _StudyPartner; }
      set
      {
        if (value != _StudyPartner)
        {
          _StudyPartner = value;
          NotifyOfPropertyChange(() => StudyPartner);
        }
      }
    }

    private StudyDataEdit _StudyData;
    public StudyDataEdit StudyData
    {
      get { return _StudyData; }
      set
      {
        if (value != _StudyData)
        {
          _StudyData = value;
          NotifyOfPropertyChange(() => StudyData);
        }
      }
    }

    private List<MultiLineTextEdit> _AllSongsCache;
    public List<MultiLineTextEdit> AllSongsCache
    {
      get { return _AllSongsCache; }
      set
      {
        if (value != _AllSongsCache)
        {
          _AllSongsCache = value;
          NotifyOfPropertyChange(() => AllSongsCache);
        }
      }
    }

    private string _FilterText;
    public string FilterText
    {
      get { return _FilterText; }
      set
      {
        if (value != _FilterText)
        {
          _FilterText = value;
          NotifyOfPropertyChange(() => FilterText);
          PopulateItems();
        }
      }
    }
    
    private bool _CheckAllToggleIsChecked;
    public bool CheckAllToggleIsChecked
    {
      get { return _CheckAllToggleIsChecked; }
      set
      {
        if (value != _CheckAllToggleIsChecked)
        {
          _CheckAllToggleIsChecked = value;
          NotifyOfPropertyChange(() => CheckAllToggleIsChecked);
          ToggleAllChecks();
        }
      }
    }

    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }

    private Visibility _ViewModelVisibility;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets all songs in DB for this user, populates Items with viewmodels for each song.
    /// </summary>
    private void SelectASong()
    {
      MultiLineTextList.GetAll((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        #region Sort MLT by Title Comparison (Comparison<MultiLineTextEdit> comparison = (a, b) =>)

        Comparison<MultiLineTextEdit> comparison = (a, b) =>
        {
          //WE'RE GOING TO TEST CHAR ORDER IN ALPHABET
          string aTitle = a.Title.ToLower();
          string bTitle = b.Title.ToLower();

          //IF THEY'RE THE SAME TITLES IN LOWER CASE, THEN THEY ARE EQUAL
          if (aTitle == bTitle)
            return 0;

          //ONLY NEED TO TEST CHARACTERS UP TO LENGTH
          int shorterTitleLength = aTitle.Length;
          bool aIsShorter = true;
          if (bTitle.Length < shorterTitleLength)
          {
            shorterTitleLength = bTitle.Length;
            aIsShorter = false;
          }

          int result = 0; //assume a and b are equal (though we know they aren't if we've reached this point)

          for (int i = 0; i < shorterTitleLength; i++)
          {
            if (aTitle[i] < bTitle[i])
            {
              result = -1;
              break;
            }
            else if (aTitle[i] > bTitle[i])
            {
              result = 1;
              break;
            }
          }

          //IF THEY ARE STILL EQUAL, THEN THE SHORTER PRECEDES THE LONGER
          if (result == 0)
          {
            if (aIsShorter)
              result = -1;
            else
              result = 1;
          }

          return result;
        };

        #endregion

        var allMultiLineTexts = r.Object;

        var songs = (from multiLineText in allMultiLineTexts
                     where multiLineText.AdditionalMetadata.Contains(MultiLineTextEdit.MetadataEntrySong)
                     select multiLineText).ToList();

        songs.Sort(comparison);

        //CACHE ALL SONGS SO WE WON'T HAVE TO DOWNLOAD THEM AGAIN.  THIS IS ASSUMING MY CURRENT NAVIGATION
        //STRUCTURE WHICH MEANS THAT YOU CAN'T ADD SONGS WITHOUT RECREATING THIS ENTIRE VIEWMODEL.
        //THIS CACHE WILL BE USED FOR FILTERING.

        AllSongsCache = songs;

        PopulateItems();
      });
    }

    private void PopulateItems()
    {
      Items.Clear();
      #region Sort MLT by Title Comparison (Comparison<MultiLineTextEdit> comparison = (a, b) =>)

        Comparison<MultiLineTextEdit> comparison = (a, b) =>
        {
          //WE'RE GOING TO TEST CHAR ORDER IN ALPHABET
          string aTitle = a.Title.ToLower();
          string bTitle = b.Title.ToLower();

          //IF THEY'RE THE SAME TITLES IN LOWER CASE, THEN THEY ARE EQUAL
          if (aTitle == bTitle)
            return 0;

          //ONLY NEED TO TEST CHARACTERS UP TO LENGTH
          int shorterTitleLength = aTitle.Length;
          bool aIsShorter = true;
          if (bTitle.Length < shorterTitleLength)
          {
            shorterTitleLength = bTitle.Length;
            aIsShorter = false;
          }

          int result = 0; //assume a and b are equal (though we know they aren't if we've reached this point)

          for (int i = 0; i < shorterTitleLength; i++)
          {
            if (aTitle[i] < bTitle[i])
            {
              result = -1;
              break;
            }
            else if (aTitle[i] > bTitle[i])
            {
              result = 1;
              break;
            }
          }

          //IF THEY ARE STILL EQUAL, THEN THE SHORTER PRECEDES THE LONGER
          if (result == 0)
          {
            if (aIsShorter)
              result = -1;
            else
              result = 1;
          }

          return result;
        };

        #endregion

      List<MultiLineTextEdit> filteredSongs = FilterSongs(AllSongsCache);
      filteredSongs.Sort(comparison);

      for (int i = 0; i < filteredSongs.Count; i++)
      {
        //CREATE THE VIEWMODEL
        var songItemViewModel = Services.Container.GetExportedValue<StudyASongItemViewModel>();

        //ASSIGN THE VIEWMODEL'S MODEL AS THE SONG
        var song = AllSongsCache[i];
        songItemViewModel.Model = song;

        //INSERT THE VIEWMODEL IN THE PROPER ORDER INTO OUR ITEMS COLLECTION
        Items.Insert(i, songItemViewModel);
      }
    }

    /// <summary>
    /// Filters songs using FilterText property.  Does NOT sort results.
    /// </summary>
    /// <param name="songs">song list to apply filter to</param>
    /// <returns>filtered song list (of type MultiLineTextEdit)</returns>
    private List<MultiLineTextEdit> FilterSongs(List<MultiLineTextEdit> songs)
    {
      if (string.IsNullOrEmpty(FilterText))
        return songs.ToList();
          
      var _filterText = FilterText;

      var filteredResults = (from song in songs
                             where (from line in song.Lines
                                    where line.Phrase.Text.Contains(FilterText)
                                    select line).Count() > 0
                             select song).ToList();

      return filteredResults;
    }

    public void AskUserForExtraDataAsync(AsyncCallback<StudyDataEdit> callback)
    {
      var askUserViewModel = Services.Container.GetExportedValue<AskUserExtraDataViewModel>();
      askUserViewModel.ShowModal(callback);
    }

    public string GetNativeLanguageText()
    {
      if (StudyData != null)
        return StudyData.NativeLanguageText;
      else
        return "";
    }

    private void ToggleAllChecks()
    {
      foreach (var viewModel in Items)
      {
        viewModel.IsChecked = CheckAllToggleIsChecked;
      }
    }

    /// <summary>
    /// Initializes ViewModel after navigated.
    /// </summary>
    /// <param name="message"></param>
    public void Handle(Navigation.EventMessages.NavigatedEventMessage message)
    {
      //WE ARE LISTENING FOR A MESSAGE THAT SAYS WE WERE SUCCESSFULLY NAVIGATED TO (SHELLVIEW.MAIN == STUDYVIEWMODEL)
      //SO WE ONLY CARE ABOUT NAVIGATED EVENT MESSAGES ABOUT OUR CORE AS DESTINATION.
      if (message.NavigationInfo.ViewModelCoreNoSpaces !=
          ViewModelBase.GetCoreViewModelName(typeof(StudyASongViewModel)))
        return;

      //WE HAVE BEEN SUCCESSFULLY NAVIGATED TO.

      //SINCE THIS IS A NONSHARED COMPOSABLE PART, WE ONLY CARE ABOUT NAVIGATED MESSAGE ONCE, SO UNSUBSCRIBE
      Services.EventAggregator.Unsubscribe(this);

      InitializeViewModel();
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public void OnImportsSatisfied()
    {
      //
    }

    #endregion
  }
}
