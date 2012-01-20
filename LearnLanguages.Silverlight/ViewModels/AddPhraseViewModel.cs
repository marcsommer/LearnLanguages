using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddPhraseViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddPhraseViewModel : ViewModelBase
  {
    public AddPhraseViewModel()
    {
      
    }

    public string LabelPhraseText { get { return ViewResources.LabelAddPhraseViewPhraseText; } }
    public string LabelLanguageText { get { return ViewResources.LabelAddPhraseViewLanguageText; } }

    private LanguageSelectorViewModel _LanguagesViewModel;
    public LanguageSelectorViewModel LanguagesViewModel
    {
      get { return _LanguagesViewModel; }
      set
      {
        if (value != _LanguagesViewModel)
        {
          _LanguagesViewModel = value;
          NotifyOfPropertyChange(() => LanguagesViewModel);
        }
      }
    }

    private PhraseEditViewModel _PhraseViewModel;
    public PhraseEditViewModel PhraseViewModel
    {
      get { return _PhraseViewModel; }
      set
      {
        if (value != _PhraseViewModel)
        {
          _PhraseViewModel = value;
          NotifyOfPropertyChange(() => PhraseViewModel);
        }
      }
    }
  }
}
