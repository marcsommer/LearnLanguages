using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.DataAccess;
using LearnLanguages.Silverlight.Interfaces;
using System.ComponentModel;

namespace LearnLanguages.Silverlight.ViewModels
{
  /// <summary>
  /// This base class provides structure for creating different views for a TranslationEdit, which 
  /// is basically a "container" class with a property Phrases of type PhraseList that does the actual containing.  
  /// This means that a TranslationEdit is successfully portrayed by a Translation container viewmodel, and 
  /// the individual phrases viewmodel.  
  /// 
  /// Descend from this class to automatically wire up these two viewmodel functions.
  /// </summary>
  /// <typeparam name="TPhrasesViewModel"></typeparam>
  //[Export(typeof(TranslationEditViewModelBase<TPhrasesViewModel>))]
  //[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class TranslationEditViewModelBase<TPhrasesViewModel> : ViewModelBase<TranslationEdit, TranslationDto>
    where TPhrasesViewModel : class, IViewModelBase, IHaveModelList<PhraseList>, INotifyPropertyChanged, new()
  {
    public TranslationEditViewModelBase()
    {
      PhrasesViewModel = new TPhrasesViewModel();
    }

    public override void SetModel(TranslationEdit model)
    {
      base.SetModel(model);
      PhrasesViewModel.ModelList = model.Phrases;
    }

    private TPhrasesViewModel _PhrasesViewModel;
    public TPhrasesViewModel PhrasesViewModel
    {
      get { return _PhrasesViewModel; }
      set
      {
        if (value != _PhrasesViewModel)
        {
          if (_PhrasesViewModel != null)
            UnhookFrom(_PhrasesViewModel);
          _PhrasesViewModel = value;
          NotifyOfPropertyChange(() => PhrasesViewModel);
          HookInto(_PhrasesViewModel);
        }
      }
    }

    private void UnhookFrom(TPhrasesViewModel phrasesViewModel)
    {
      phrasesViewModel.PropertyChanged -= HandlePhrasesViewModelPropertyChanged;
    }

    private void HookInto(TPhrasesViewModel phrasesViewModel)
    {
      phrasesViewModel.PropertyChanged += HandlePhrasesViewModelPropertyChanged;
    }

    void HandlePhrasesViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(() => Model);
    }


  }
}
