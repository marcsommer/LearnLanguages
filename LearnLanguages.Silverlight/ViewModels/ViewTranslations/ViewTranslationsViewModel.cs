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
using System.Collections.Generic;

namespace LearnLanguages.Silverlight.ViewModels
{

  /// <summary>
  /// This contains Items that each represent a Translation.  
  /// A Translation contains Phrases, so each Item in this collection is itself a collection.
  /// </summary>
  [Export(typeof(ViewTranslationsViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ViewTranslationsViewModel : Conductor<ViewTranslationsItemViewModel>.Collection.AllActive, 
                                           Interfaces.IViewModelBase
  {
    #region Ctors and Init

    public ViewTranslationsViewModel()
    {
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
      TranslationList.GetAll((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        var allTranslations = r.Object;
        ModelList = allTranslations;
        PopulateViewModels(allTranslations);
      });
    }
    
    #endregion

    #region Properties

    private TranslationList _ModelList;
    public TranslationList ModelList
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
          NotifyOfPropertyChange(() => CanSave);
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

    private string _FilterLabel = AppResources.FilterLabel;
    public string FilterLabel
    {
      get { return _FilterLabel; }
      set
      {
        if (value != _FilterLabel)
        {
          _FilterLabel = value;
          NotifyOfPropertyChange(() => FilterLabel);
        }
      }
    }

    private string _FilterText = "";
    public string FilterText
    {
      get { return _FilterText; }
      set
      {
        if (value != _FilterText)
        {
          _FilterText = value;
          PopulateViewModels(ModelList);
          NotifyOfPropertyChange(() => FilterText);
        }
      }
    }

    #endregion

    #region Methods

    private IEnumerable<TranslationEdit> FilterTranslations(TranslationList translations)
    {
      if (string.IsNullOrEmpty(FilterLabel))
        return translations;

      var results = from translation in translations
                    where (from phrase in translation.Phrases
                           where phrase.Text.Contains(FilterText)
                           select phrase).Count() > 0
                    select translation;

      return results;
    }

    private void PopulateViewModels(TranslationList allTranslations)
    {
      //CLEAR OUR ITEMS (VIEWMODELS)
      Items.Clear();

      //FILTER OUR TRANSLATIONS
      var filteredTranslations = FilterTranslations(allTranslations);

      //POPULATE NEW VIEWMODELS WITH FILTERED RESULTS
      foreach (var translationEdit in filteredTranslations)
      {
        var itemViewModel = Services.Container.GetExportedValue<ViewTranslationsItemViewModel>();
        itemViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
        itemViewModel.Model = translationEdit;
        Items.Add(itemViewModel);
      }
    }

    protected virtual void HookInto(TranslationList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(TranslationList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void HandleChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      //Model.BeginSave((s2, r2) =>
      //{
      //  if (r2.Error != null)
      //    throw r2.Error;
      //  Model = (TranslationList)r2.NewObject;
      //});
      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);

      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => this.ModelList);
      NotifyOfPropertyChange(() => CanSave);
      //NotifyOfPropertyChange(() => CanCancelEdit);
    }
    protected void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
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
      Publish.PartSatisfied("ViewTranslations");
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
          throw r.Error;

        ModelList = (TranslationList)r.NewObject;
        //propagate new TranslationEdits to their ViewModels
        PopulateViewModels(ModelList);

        NotifyOfPropertyChange(() => CanSave);
      });
    }

    public bool CanInitiateDeleteChecked
    {
      get
      {
        bool somethingIsChecked = (from viewModel in Items
                                   where ((ViewTranslationsItemViewModel)viewModel).IsChecked
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
      foreach (var item in Items)
      {
        var vm = (ViewTranslationsItemViewModel)item;
        vm.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }

    #endregion
  }
}
