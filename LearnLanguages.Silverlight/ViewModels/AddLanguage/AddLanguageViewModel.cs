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
