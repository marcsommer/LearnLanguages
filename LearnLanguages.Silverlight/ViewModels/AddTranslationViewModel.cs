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
  [Export(typeof(AddTranslationViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class AddTranslationViewModel : ViewModelBase
  {
    public AddTranslationViewModel()
    {
      BlankTranslationRetriever.CreateNew((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          var retriever = r.Object;

          TranslationViewModel = Services.Container.GetExportedValue<TranslationEditViewModel>();
          TranslationViewModel.Model = retriever.Translation;
        });
    }

    private TranslationEditViewModel _TranslationViewModel;
    public TranslationEditViewModel TranslationViewModel
    {
      get { return _TranslationViewModel; }
      set
      {
        if (value != _TranslationViewModel)
        {
          _TranslationViewModel = value;
          NotifyOfPropertyChange(() => TranslationViewModel);
        }
      }
    }
  }
}
