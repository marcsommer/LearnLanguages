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
  [Export(typeof(ViewPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  //public class ViewPhrasesViewModel : ViewModelBase<PhraseList, PhraseEdit, PhraseDto>
  public class ViewPhrasesViewModel : Conductor<ViewModelBase>.Collection.AllActive, 
                                      Interfaces.IViewModelBase
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
          Model = allPhrases;
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

    private void PopulateViewModels(PhraseList allPhrases)
    {
      Items.Clear();
      foreach (var phraseEdit in allPhrases)
      {
        var itemViewModel = Services.Container.GetExportedValue<ViewPhrasesItemViewModel>();
        itemViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
        itemViewModel.Model = phraseEdit;
        Items.Add(itemViewModel);
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
      Publish.PartSatisfied("ViewPhrases");
    }

    private PhraseList _ModelList;
    public PhraseList Model
    {
      get { return _ModelList; }
      set
      {
        if (value != _ModelList)
        {
          UnhookFrom(_ModelList);
          _ModelList = value;
          HookInto(_ModelList);
          NotifyOfPropertyChange(() => Model);
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
      NotifyOfPropertyChange(() => this.Model);
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
      NotifyOfPropertyChange(() => this.Model);
      NotifyOfPropertyChange(() => CanSave);
      //NotifyOfPropertyChange(() => CanCancelEdit);
    }

    public bool CanSave
    {
      get
      {
        return (Model != null && Model.IsSavable);
      }
    }
    public virtual void Save()
    {
      Model.BeginSave((s, r) =>
      {
        if (r.Error != null)
          throw r.Error;

        Model = (PhraseList)r.NewObject;
        //propagate new PhraseEdits to their ViewModels
        PopulateViewModels(Model);

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
                                   where ((ViewPhrasesItemViewModel)viewModel).IsChecked
                                   select viewModel).Count() > 0;

        return somethingIsChecked && CanSave;
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
                               where ((ViewPhrasesItemViewModel)viewModel).IsChecked
                               select (ViewPhrasesItemViewModel)viewModel;

      foreach (var toDelete in checkedForDeletion)
      {
        Model.Remove(toDelete.Model);
      }

      NotifyOfPropertyChange(() => CanSave);
      NotifyOfPropertyChange(() => Model);
      if (CanSave)
        Save();

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
    }

    public void CancelDeleteChecked()
    {
      foreach (var item in Items)
      {
        var vm = (ViewPhrasesItemViewModel)item;
        vm.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }
  }
}
