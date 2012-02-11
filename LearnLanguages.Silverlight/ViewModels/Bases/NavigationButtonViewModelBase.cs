using System;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Navigation;

namespace LearnLanguages.Silverlight.ViewModels
{
  public abstract class NavigationButtonViewModelBase : ViewModelBase
  {
    /// <summary>
    /// Text that will display on the button.  Defaults to this 
    /// NavigationButton's target view model core name. Override GetLabelText()
    /// for a custom label text.
    /// </summary>
    public string LabelText
    {
      get 
      {
        return GetLabelText();
      }
    }
    /// <summary>
    /// Text that will display on the button.  Defaults to this 
    /// NavigationButton's target view model core name. Override GetLabelText()
    /// for a custom label text.
    /// </summary>
    protected virtual string GetLabelText()
    {
      var text = GetNavigationTargetViewModelCoreName(true);
      return text;
    }

    /// <summary>
    /// Use this to get the target viewmodel's core name.
    /// Uses Convention "[CoreName]NavigationButtonViewModel".  Returns text, adding spaces
    /// before any capital letters if withSpaces == true.  
    /// E.g. AddNewNavigationButtonViewModel returns "Add New" (with a space).  
    /// Override this to set a custom viewmodel core name.
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
    /// <summary>
    /// Publishes a NavigationRequest to the TargetViewModelCoreName.
    /// </summary>
    public virtual void Navigate()
    {
      var navInfo = new NavigationInfo(Guid.NewGuid(), GetNavigationTargetViewModelCoreName(false), AppResources.BaseAddress);
      Services.EventAggregator.Publish(new Navigation.EventMessages.NavigationRequestedEventMessage(navInfo));
    }
  }
}
