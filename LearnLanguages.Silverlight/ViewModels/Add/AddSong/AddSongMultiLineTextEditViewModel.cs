using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Common.ViewModelBases;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddSongMultiLineTextEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddSongMultiLineTextEditViewModel : ViewModelBase<PhraseEdit, PhraseDto>
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

    private void HandleLanguageChanged(object sender, EventArgs e)
    {
      if (sender != null) 
        Model.Language = ((LanguageEditViewModel)sender).Model;
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
    
    public string LabelSongText { get { return ViewViewModelResources.LabelSongTextAddSong; } }
    public string LabelLanguageText { get { return ViewViewModelResources.LabelLanguageAddSong; } }

    public string InstructionsSelectLanguage { get { return ViewViewModelResources.InstructionsSelectLanguageAddSong; } }
    public string InstructionsEnterSongText { get { return ViewViewModelResources.InstructionsEnterSongTextAddSong; } }

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

    
  }
}
