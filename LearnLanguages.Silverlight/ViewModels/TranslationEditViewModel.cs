using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(TranslationEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class TranslationEditViewModel : ViewModelBase<TranslationEdit, TranslationDto>
  {
    public TranslationEditViewModel()
    {
    }

  }
}
