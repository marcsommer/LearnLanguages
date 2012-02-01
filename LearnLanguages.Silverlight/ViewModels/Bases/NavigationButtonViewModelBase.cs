using System;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Navigation;

namespace LearnLanguages.Silverlight.ViewModels
{
  public abstract class NavigationButtonViewModelBase : ViewModelBase
  {
    public string LabelText
    {
      get 
      {
        var text = GetNavigationTargetViewModelCoreName(true);
        return text;
      }
    }
    /// <summary>
    /// Uses Convention "[Text]NavigationButtonViewModel".  Returns text, adding spaces
    /// before any capital letters.  E.g. AddNewNavigationButtonViewModel returns "Add New" 
    /// (with a space).  
    /// </summary>
    /// <returns></returns>
    public virtual string GetNavigationTargetViewModelCoreName(bool withSpaces)
    {
      var viewModelCoreName = ViewModelBase.GetCoreViewModelName(this.GetType(), withSpaces);
      string coreMinusNavigationButtonText = "";
      if (withSpaces)
        coreMinusNavigationButtonText = viewModelCoreName.Replace(" Navigation Button", string.Empty); //NB space before Navigation
      else 
        coreMinusNavigationButtonText = viewModelCoreName.Replace("NavigationButton", string.Empty); //NB space before Navigation
      return coreMinusNavigationButtonText;
    }
    public bool CanNavigate
    {
      get { return CanNavigateImpl(); }
    }
    public abstract bool CanNavigateImpl();
    public virtual void Navigate()
    {
      var navInfo = new NavigationInfo(Guid.NewGuid(), GetNavigationTargetViewModelCoreName(false), AppResources.BaseAddress);
      Services.EventAggregator.Publish(new Navigation.EventMessages.NavigationRequestedEventMessage(navInfo));
    }
  }
}
