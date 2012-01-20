using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LanguageReadOnlyViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class LanguageReadOnlyViewModel : ViewModelBase
  {
    private LanguageEdit _LanguageEdit;
    public LanguageEdit LanguageEdit
    {
      get { return _LanguageEdit; }
      set
      {
        if (value != _LanguageEdit)
        {
          _LanguageEdit = value;
          NotifyOfPropertyChange(() => LanguageEdit);
          NotifyOfPropertyChange(() => LanguageText);
        }
      }
    }

    public string LanguageText
    {
      get { return LanguageEdit.Text; }
    }
  }
}
