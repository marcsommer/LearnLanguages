using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(PhraseEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class PhraseEditViewModel : ViewModelBase<PhraseEdit, PhraseDto>
  {
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
    public string LabelPhraseText { get { return ViewViewModelResources.LabelAddPhraseViewPhraseText; } }
    public string LabelLanguageText { get { return ViewViewModelResources.LabelAddPhraseViewLanguageText; } }
  }
}
