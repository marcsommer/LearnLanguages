using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.Windows;
using LearnLanguages.Common.ViewModelBases;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(SelectNativeLanguageViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class SelectNativeLanguageViewModel : ViewModelBase<StudyDataEdit, StudyDataDto>
  {
    public SelectNativeLanguageViewModel()
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
        NativeLanguage = ((LanguageEditViewModel)sender).Model;
    }

    private LanguageEdit _NativeLanguage;
    public LanguageEdit NativeLanguage
    {
      get { return _NativeLanguage; }
      set
      {
        if (value != _NativeLanguage)
        {
          _NativeLanguage = value;
          NotifyOfPropertyChange(() => NativeLanguage);
          if (_NativeLanguage != null && !string.IsNullOrEmpty(_NativeLanguage.Text))
            EventMessages.NativeLanguageChangedEventMessage.Publish(_NativeLanguage.Text);
        }
      }
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

    public string LabelLanguageText { get { return ViewViewModelResources.LabelSelectNativeLanguageLanguageText; } }
    public string InstructionsSelectLanguage 
    { 
      get 
      { 
        return ViewViewModelResources.InstructionsSelectNativeLanguageSelectLanguage; 
      } 
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

    private bool _HasBeenSaved;
    public bool HasBeenSaved
    {
      get { return _HasBeenSaved; }
      set
      {
        if (value != _HasBeenSaved)
        {
          _HasBeenSaved = value;
          NotifyOfPropertyChange(() => HasBeenSaved);
          NotifyOfPropertyChange(() => CanSave);
        }
      }
    }

    public bool CanSave
    {
      get
      {
        return (NativeLanguage != null &&
                !string.IsNullOrEmpty(NativeLanguage.Text) &&
                !HasBeenSaved);
      }
    }
    public void Save()
    {
      EventMessages.NativeLanguageChangedEventMessage.Publish(NativeLanguage.Text);
    }

    #region Base
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    public void OnImportsSatisfied()
    {
      //
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
  }
}
