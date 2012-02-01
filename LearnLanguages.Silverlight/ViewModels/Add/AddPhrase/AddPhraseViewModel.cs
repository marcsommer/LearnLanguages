using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;

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
