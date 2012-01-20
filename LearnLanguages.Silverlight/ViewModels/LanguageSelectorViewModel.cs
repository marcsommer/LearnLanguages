using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LanguageSelectorViewModel))]
  public class LanguageSelectorViewModel : Conductor<LanguageEditViewModel>.Collection.OneActive, Interfaces.IViewModelBase
  {
    public LanguageSelectorViewModel()
    {
      LanguageList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var allLanguages = r.Object;
          foreach (var language in allLanguages)
          {
            var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
            languageViewModel.Model = language;
            Items.Add(languageViewModel);
          }
        });
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public void OnImportsSatisfied()
    {
      Events.Publish.PartSatisfied(ViewModelBase.GetCoreViewModelName(GetType()));
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
  }
}
