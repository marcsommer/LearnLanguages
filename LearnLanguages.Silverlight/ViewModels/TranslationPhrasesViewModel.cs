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
  [Export(typeof(TranslationPhrasesViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class TranslationPhrasesViewModel : Conductor<ViewModelBase>.Collection.AllActive, 
                                             Interfaces.IViewModelBase
  {
    #region Ctors and Init

    public TranslationPhrasesViewModel()
    {
      _InitiateDeleteVisibility = Visibility.Visible;
      _FinalizeDeleteVisibility = Visibility.Collapsed;
      //PhraseList.NewPhraseList((s, r) =>
      ////PhraseList.GetAll((s, r) =>
      //{
      //  if (r.Error != null)
      //    throw r.Error;

      //  var allPhrases = r.Object;
      //  Model = allPhrases;
      //  //PopulateViewModels(allPhrases);
      //  //hack: loading viewmodels changes the phraseedit.language, so we have to save immediately to make clean.
      //  //Model.BeginSave((s2, r2) =>
      //  //  {
      //  //    if (r2.Error != null)
      //  //      throw r2.Error;
      //  //    Model = (PhraseList)r2.NewObject;
      //  //  });
      //});
    }

    #endregion

    #region Fields

    #endregion

    #region Properties

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
          PopulateViewModels(_ModelList);
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

    public void AddPhrase(PhraseEdit phrase)
    {
      ModelList.Add(phrase);
    }


    public bool LoadFromUri(Uri uri)
    {
      return true;
    }

    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }

    private void PopulateViewModels(PhraseList allPhrases)
    {
      Items.Clear();
      foreach (var phraseEdit in allPhrases)
      {
        var itemViewModel = Services.Container.GetExportedValue<TranslationPhrasesItemViewModel>();
        itemViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(HandleItemViewModelChanged);
        itemViewModel.Model = phraseEdit;
        Items.Add(itemViewModel);
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
    

    #endregion

    #region Events

    void HandleItemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
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
    public void OnImportsSatisfied()
    {
      Publish.PartSatisfied("ViewPhrases");
    }


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

        ModelList = (PhraseList)r.NewObject;
        //propagate new PhraseEdits to their ViewModels
        PopulateViewModels(ModelList);

        NotifyOfPropertyChange(() => CanSave);
      });
    }

    public bool CanInitiateDeleteChecked
    {
      get
      {
        bool somethingIsChecked = (from viewModel in Items
                                   where ((TranslationPhrasesItemViewModel)viewModel).IsChecked
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
                               where ((TranslationPhrasesItemViewModel)viewModel).IsChecked
                               select (TranslationPhrasesItemViewModel)viewModel;

      foreach (var toDelete in checkedForDeletion)
      {
        ModelList.Remove(toDelete.Model);
      }

      Save();

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
    }

    public void CancelDeleteChecked()
    {
      foreach (var item in Items)
      {
        var vm = (TranslationPhrasesItemViewModel)item;
        vm.IsChecked = false;
      }

      InitiateDeleteVisibility = Visibility.Visible;
      FinalizeDeleteVisibility = Visibility.Collapsed;
      NotifyOfPropertyChange(() => CanInitiateDeleteChecked);
    }


    #endregion
  }
}
