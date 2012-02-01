using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using Caliburn.Micro;
using System.Collections.Specialized;
using System.Windows;
using System.Collections.Generic;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(ViewPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class ViewPhrasesViewModel : ViewModelBase<PhraseList, PhraseEdit, PhraseDto>
  public class ViewPhrasesViewModel : Conductor<ViewPhrasesItemViewModel>.Collection.AllActive, 
                                      IViewModelBase
  {
    public ViewPhrasesViewModel()
    {
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
      PhraseList.GetAll((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var allPhrases = r.Object;
          ModelList = allPhrases;
          PopulateViewModels(allPhrases);
          //hack: loading viewmodels changes the phraseedit.language, so we have to save immediately to make clean.
          //Model.BeginSave((s2, r2) =>
          //  {
          //    if (r2.Error != null)
          //      throw r2.Error;
          //    Model = (PhraseList)r2.NewObject;
          //  });
        });
    }

    private void PopulateViewModels(PhraseList phrases)
    {
      Items.Clear();
      var filteredPhrases = FilterPhrases(phrases);
      foreach (var phraseEdit in filteredPhrases)
      {
        var itemViewModel = Services.Container.GetExportedValue<ViewPhrasesItemViewModel>();
        itemViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
        itemViewModel.Model = phraseEdit;
        Items.Add(itemViewModel);
      }
    }

    private IEnumerable<PhraseEdit> FilterPhrases(PhraseList phrases)
    {
      if (string.IsNullOrEmpty(FilterLabel))
        return phrases;

      var results = from phrase in phrases
                    where phrase.Text.Contains(FilterText)
                    select phrase;

      return results;
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

    void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
      //LearnLanguages.Navigation.Publish.PartSatisfied("ViewPhrases");
    }

    private PhraseList _ModelList;
    public PhraseList ModelList
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

    protected virtual void HookInto(PhraseList modelList)
    {
      if (modelList != null)
      {
        modelList.CollectionChanged += HandleCollectionChanged;
        modelList.ChildChanged += HandleChildChanged;
      }
    }
    protected virtual void UnhookFrom(PhraseList modelList)
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
      //  Model = (PhraseList)r2.NewObject;
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

        ModelList = (PhraseList)r.NewObject;
        //propagate new PhraseEdits to their ViewModels
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
    //  foreach (var phraseEdit in Model)
    //  {
    //    if (phraseEdit.IsDirty)
    //    {
    //      var initialEditLevel = phraseEdit.GetEditLevel();
    //      for (int i = 0; i < initialEditLevel; i++)
    //      {
    //        phraseEdit.CancelEdit();
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
      foreach (var phraseItemViewModel in Items)
      {
        phraseItemViewModel.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }
  }
}
