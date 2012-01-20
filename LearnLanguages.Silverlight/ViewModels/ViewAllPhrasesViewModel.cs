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
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddPhraseViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddPhraseViewModel : ViewModelBase
  {
    public AddPhraseViewModel()
    {
      PhraseEdit.NewPhraseEdit((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var phraseViewModel = Services.Container.GetExportedValue<PhraseEditViewModel>();
          phraseViewModel.Model = r.Object;
          Phrase = phraseViewModel;
        });
    }

    private PhraseEditViewModel _Phrase;
    public PhraseEditViewModel Phrase
    {
      get { return _Phrase; }
      set
      {
        if (value != _Phrase)
        {
          _Phrase = value;
          NotifyOfPropertyChange(() => Phrase);
        }
      }
    }
  }
}
