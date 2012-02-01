using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(AddLanguageViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddLanguageViewModel : ViewModelBase
  {
    public AddLanguageViewModel()
    {
      LanguageEdit.NewLanguageEdit((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var languageViewModel = Services.Container.GetExportedValue<AddLanguageLanguageEditViewModel>();
          languageViewModel.Model = r.Object;
          LanguageViewModel = languageViewModel;
        });
    }

    private AddLanguageLanguageEditViewModel _LanguageViewModel;
    public AddLanguageLanguageEditViewModel LanguageViewModel
    {
      get { return _LanguageViewModel; }
      set
      {
        if (value != _LanguageViewModel)
        {
          _LanguageViewModel = value;
          NotifyOfPropertyChange(() => LanguageViewModel);
        }
      }
    }
  }
}
