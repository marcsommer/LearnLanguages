using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Business;
using System.Linq;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(LanguageSelectorViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class LanguageSelectorViewModel : Conductor<LanguageEditViewModel>.Collection.OneActive, Interfaces.IViewModelBase
  public class LanguageSelectorViewModel : ViewModelBase
  {
    public LanguageSelectorViewModel()
    {
      Items = new BindableCollection<LanguageEditViewModel>();

      LanguageList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var allLanguages = r.Object;
          foreach (var languageEdit in allLanguages)
          {
            var languageViewModel = Services.Container.GetExportedValue<LanguageEditViewModel>();
            languageViewModel.Model = languageEdit;
            Items.Add(languageViewModel);
          }

          if (SelectedItem != null && SelectedItem.Model != null)
          {
            //our currently selected item is not the actual item in the list, as it is being set
            //before our list is getting populated.
            SelectedItem = (from language in Items
                            where language.Model.Id == SelectedItem.Model.Id
                            select language).FirstOrDefault();
          }

          //SelectedItem = Items[0];
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

    private LanguageEditViewModel _SelectedItem;
    public LanguageEditViewModel SelectedItem
    {
      get { return _SelectedItem; }
      set
      {
        if (value != _SelectedItem)
        {
          _SelectedItem = value;
          NotifyOfPropertyChange(() => SelectedItem);
          if (SelectedItemChanged != null)
            SelectedItemChanged(_SelectedItem, EventArgs.Empty);
        }
      }
    }

    private BindableCollection<LanguageEditViewModel> _Items;
    public BindableCollection<LanguageEditViewModel> Items
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

    public event EventHandler SelectedItemChanged;

    private bool _IsEnabled = true;
    public bool IsEnabled
    {
      get { return _IsEnabled; }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;
          NotifyOfPropertyChange(() => IsEnabled);
        }
      }
    }

    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public void OnImportsSatisfied()
    {
      //Navigation.Publish.PartSatisfied(ViewModelBase.GetCoreViewModelName(GetType()));
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
  }
}
