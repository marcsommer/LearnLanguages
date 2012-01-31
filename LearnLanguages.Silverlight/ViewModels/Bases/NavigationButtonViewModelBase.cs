using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LearnLanguages.Silverlight.Interfaces;

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
      var navInfo = new NavigationInfo(Guid.NewGuid(), GetNavigationTargetViewModelCoreName(false));
      Services.EventAggregator.Publish(new Events.NavigationRequestedEventMessage(navInfo));
    }
  }
}
