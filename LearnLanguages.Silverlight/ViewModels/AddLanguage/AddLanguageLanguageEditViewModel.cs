using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddLanguageLanguageEditViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddLanguageLanguageEditViewModel : LanguageEditViewModel
  {
    public string LabelLanguageText { get { return ViewViewModelResources.LabelAddLanguageViewLanguageText; } }
  }
}
