using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using System.Windows.Controls;
using Caliburn.Micro;
using LearnLanguages.Silverlight.Events;
using System.Collections.Specialized;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewLanguagesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class ViewLanguagesViewModel : ViewModelBase<LanguageList, LanguageEdit, LanguageDto>
  public class ViewLanguagesViewModel : Conductor<ViewLanguagesItemViewModel>.Collection.AllActive, 
                                        Interfaces.IViewModelBase
  {
    #region Ctors and Init

    public ViewLanguagesViewModel()
    {
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
      LanguageList.GetAll((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        var allLanguages = r.Object;
        ModelList = allLanguages;
        PopulateViewModels(allLanguages);
      });
    }

    #endregion
    
    #region Fields

    #endregion

    #region Properties

    private LanguageList _ModelList;
    public LanguageList ModelList
    {
      get { return _ModelList; }
      set
      {
        if (value != _ModelList)
        {
          UnhookFrom(_ModelList);
          _ModelList = value;
          HookInto(_ModelList);
          NotifyOfPropertyChange(() => ModelList);
        }
      }
    }

    private Visibility _InitiateDeleteVisibility;
    public Visibility InitiateDeleteVisibility
    {
      get { return _InitiateDeleteVisibility; }
      set
      {
        if (value != _InitiateDeleteVisibility)
        {
          _InitiateDeleteVisibility = value;
          NotifyOfPropertyChange(() => InitiateDeleteVisibility);
        }
      }
    }

    private Visibility _FinalizeDeleteVisibility;
    public Visibility FinalizeDeleteVisibility
    {
      get { return _FinalizeDeleteVisibility; }
      set
      {
        if (value != _FinalizeDeleteVisibility)
        {
          _FinalizeDeleteVisibility = value;
          NotifyOfPropertyChange(() => FinalizeDeleteVisibility);
        }
      }
    }

    #endregion

    #region Methods

    private void PopulateViewModels(LanguageList allLanguages)
    {
      UnhookFromAllLanguages();

      Items.Clear();
      foreach (var languageEdit in allLanguages)
      {
        var itemViewModel = Services.Container.GetExportedValue<ViewLanguagesItemViewModel>();
        HookInto(itemViewModel);
        itemViewModel.Model = languageEdit;
        Items.Add(itemViewModel);
      }
    }
    private void UnhookFromAllLanguages()
    {
      foreach (var languageViewModel in Items)
      {
        UnhookFrom(languageViewModel);
      }
    }
    private void HookInto(ViewLanguagesItemViewModel itemViewModel)
    {
      if (itemViewModel != null)
        itemViewModel.PropertyChanged += HandleItemViewModelChanged;
    }
    private void UnhookFrom(ViewLanguagesItemViewModel itemViewModel)
    {
      if (itemViewModel != null)
        itemViewModel.PropertyChanged -= HandleItemViewModelChanged;
    }

    protected virtual void HookInto(LanguageList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(LanguageList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }

    protected void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }
    protected virtual void HandleChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }


    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }
    public void OnImportsSatisfied()
    {
      Publish.PartSatisfied("ViewLanguages");
    }

    #endregion

    #region Events

    
    #endregion

    #region Commands

    public bool CanSave
    {
      get
      {
        return (ModelList != null && ModelList.IsSavable);
      }
    }
    public virtual void Save()
    {
      ModelList.BeginSave((s, r) =>
      {
        if (r.Error != null)
        {
          throw r.Error;
        }

        ModelList = (LanguageList)r.NewObject;
        //propagate new LanguageEdits to their ViewModels
        PopulateViewModels(ModelList);

        NotifyOfPropertyChange(() => CanSave);
      });
    }

//public bool CanCancelEdit
    //{
    //  get
    //  {
    //    return (Model != null && Model.IsDirty);
    //  }
    //}
    //public virtual void CancelEdit()
    //{
    //  foreach (var languageEdit in Model)
    //  {
    //    if (languageEdit.IsDirty)
    //    {
    //      var initialEditLevel = languageEdit.GetEditLevel();
    //      for (int i = 0; i < initialEditLevel; i++)
    //      {
    //        languageEdit.CancelEdit();
    //      }
    //    }
    //  }
    //  //Model.CancelEdit();
    //  NotifyOfPropertyChange(() => CanCancelEdit);
    //  NotifyOfPropertyChange(() => CanSave);
    //}

    public bool CanInitiateDeleteChecked
    {
      get
      {
        bool somethingIsChecked = (from viewModel in Items
                                   where viewModel.IsChecked
                                   select viewModel).Count() > 0;

        return somethingIsChecked;
      }
    }
    public void InitiateDeleteChecked()
    {
      InitiateDeleteVisibility = Visibility.Collapsed;
      FinalizeDeleteVisibility = Visibility.Visible;
    }
    public void FinalizeDeleteChecked()
    {
      var checkedForDeletion = from viewModel in Items
                               where viewModel.IsChecked
                               select viewModel;

      foreach (var toDelete in checkedForDeletion)
      {
        ModelList.Remove(toDelete.Model);
      }

      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => ModelList);
      if (CanSave)
        Save();

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
    }
    public void CancelDeleteChecked()
    {
      foreach (var languageItemViewModel in Items)
      {
        languageItemViewModel.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }

    #endregion
  }
}
