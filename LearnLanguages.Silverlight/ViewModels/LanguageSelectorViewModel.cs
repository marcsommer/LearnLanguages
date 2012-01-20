using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LanguageSelectorViewModel))]
  //public class LanguageSelectorViewModel : Conductor<LanguageEditViewModel>.Collection.OneActive, Interfaces.IViewModelBase
  public class LanguageSelectorViewModel : ViewModelBase
  {
    public LanguageSelectorViewModel()
    {
      LanguageList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var allLanguages = r.Object;
          Items = new BindableCollection<LanguageEdit>(allLanguages);
          SelectedItem = Items[0];
          //foreach (var language in allLanguages)
          //{
          //  //var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
          //  //languageViewModel.Model = language;
          //  //Items.Add(languageViewModel);
          //  Items.Add(language);
          //}
          
          //ChangeActiveItem(Items[0], true);
        });
    }

    private LanguageEdit _SelectedItem;
    public LanguageEdit SelectedItem
    {
      get { return _SelectedItem; }
      set
      {
        if (value != _SelectedItem)
        {
          _SelectedItem = value;
          NotifyOfPropertyChange(() => SelectedItem);
        }
      }
    }

    private BindableCollection<LanguageEdit> _Items;
    public BindableCollection<LanguageEdit> Items
    {
      get { return _Items; }
      set
      {
        if (value != _Items)
        {
          _Items = value;
          NotifyOfPropertyChange(() => Items);
        }
      }
    }

    //private LanguageEdit _SelectedLanguage;
    //public LanguageEdit SelectedLanguage
    //{
    //  get { return _SelectedLanguage; }
    //  set
    //  {
    //    if (value != _SelectedLanguage)
    //    {
    //      _SelectedLanguage = value;
    //      NotifyOfPropertyChange(() => SelectedLanguage);
    //    }
    //  }
    //}

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
