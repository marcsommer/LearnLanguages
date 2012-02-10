using System;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Navigation;

namespace LearnLanguages.Silverlight.ViewModels
{
  public abstract class NavigationSetTitleViewModelBase : ViewModelBase,
                                                          Interfaces.ICanExpand
  {
    public string LabelText
    {
      get { return GetLabelText(); }
    }
    public abstract string GetLabelText();
 
    public bool CanToggleExpanded
    {
      get { return CanToggleExpandedImpl(); }
    }
    /// <summary>
    /// Default behavior is to always return true;
    /// </summary>
    /// <returns></returns>
    public virtual bool CanToggleExpandedImpl()
    {
      if (IsExpanded)
        return false;
      else
        return true;
    }
    public virtual void ToggleExpanded()
    {
      IsExpanded = !IsExpanded;
      EventMessages.ExpandedChangedEventMessage.Publish(this);
    }

    private bool _IsExpanded;
    public bool IsExpanded
    {
      get { return _IsExpanded; }
      set
      {
        if (value != _IsExpanded)
        {
          _IsExpanded = value;
          NotifyOfPropertyChange(() => IsExpanded);
        }
      }
    }
  }
}
