using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System.Windows;

namespace LearnLanguages.Silverlight.ViewModels
{
  [Export(typeof(NavigationSetViewModel))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class NavigationSetViewModel : Conductor<IViewModelBase>.Collection.AllActive,
                                        IViewModelBase
  {
    public NavigationSetViewModel()
    {
      _ShowItems = false;
      _ViewModelVisibility = Visibility.Visible;
      RefreshItemsVisibility();
    }

    private NavigationSetTitleViewModelBase _TitleControl;
    public NavigationSetTitleViewModelBase TitleControl
    {
      get { return _TitleControl; }
      set
      {
        if (value != _TitleControl)
        {
          _TitleControl = value;
          NotifyOfPropertyChange(() => TitleControl);
        }
      }
    }

    public void AddControl(int index, IViewModelBase controlViewModel)
    {
      Items.Insert(index, controlViewModel);

      RefreshItemsVisibility();
    }

    /// <summary>
    /// Adds the controlViewModels (NavButtons, Info viewmodels, etc) by their index in the ordered list.
    /// So, list[0] is Items[0] and so forth.  
    /// </summary>
    public void AddControls(IList<ViewModelBase> orderedControlViewModels, bool clearExistingItems = true)
    {
      if (clearExistingItems)
        Items.Clear();

      for (int i = 0; i < orderedControlViewModels.Count; i++)
        Items.Insert(i, orderedControlViewModels[i]);

      RefreshItemsVisibility();
    }

    public void OnImportsSatisfied()
    {
      //var coreViewModelName = ViewModelBase.GetCoreViewModelName(typeof(NavigationSetViewModel));
      //Services.EventAggregator.Publish(new Events.PartSatisfiedEventMessage(coreViewModelName));
    }
    public bool LoadFromUri(Uri uri)
    {
      return true;
    }
    public bool ShowGridLines
    {
      get { return bool.Parse(AppResources.ShowGridLines); }
    }

    private Visibility _ViewModelVisibility;
    public Visibility ViewModelVisibility
    {
      get { return _ViewModelVisibility; }
      set
      {
        if (value != _ViewModelVisibility)
        {
          _ViewModelVisibility = value;
          NotifyOfPropertyChange(() => ViewModelVisibility);
        }
      }
    }

    private bool _ShowItems;
    public bool ShowItems
    {
      get { return _ShowItems; }
      set
      {
        if (value != _ShowItems)
        {
          _ShowItems = value;
          NotifyOfPropertyChange(() => ShowItems);
          RefreshItemsVisibility();
        }
      }
    }

    private Visibility _ItemsVisibility;
    public Visibility ItemsVisibility
    {
      get { return _ItemsVisibility; }
      set
      {
        if (value != _ItemsVisibility)
        {
          _ItemsVisibility = value;
          NotifyOfPropertyChange(() => ItemsVisibility);
        }
      }
    }

    private void RefreshItemsVisibility()
    {
      if (ShowItems)
        ItemsVisibility = Visibility.Visible;
      else
        ItemsVisibility = Visibility.Collapsed;

      foreach (var viewModelControl in Items)
      {
        if (ShowItems)
          viewModelControl.ViewModelVisibility = Visibility.Visible;
        else
          viewModelControl.ViewModelVisibility = Visibility.Collapsed;
      }
    }
  }
}
