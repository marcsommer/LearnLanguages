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
      //TranslationEdit.NewTranslationEdit((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;
          var retriever = r.Object;

          var translationViewModel = Services.Container.GetExportedValue<TranslationEditViewModel>();
          translationViewModel.Model = retriever.Translation;

            //translationViewModel.Model = r.Object;
          Translation = translationViewModel;

          Translation.Model.Phrases.AddedNew += (s2, r2) =>
            {
              var phraseEdit = r2.NewObject;
              phraseEdit.Username = Csla.ApplicationContext.User.Identity.Name;

            };
        });
    }

    private TranslationEditViewModel _Translation;
    public TranslationEditViewModel Translation
    {
      get { return _Translation; }
      set
      {
        if (value != _Translation)
        {
          _Translation = value;
          NotifyOfPropertyChange(() => Translation);
        }
      }
    }
  }
}
