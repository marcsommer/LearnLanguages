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

namespace LearnLanguages.Silverlight.Events
{
  public static class Publish
  {
    public static void NavigationRequest<T>() where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(Guid.NewGuid());
      Services.EventAggregator.Publish(new Events.NavigationRequestedEventMessage(navInfo));
    }
    public static void Navigating<T>(Guid navigationId) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(navigationId);
      Services.EventAggregator.Publish(new Events.NavigatingEventMessage(navInfo));
    }
    public static void Navigated<T>(Guid navigationId) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(navigationId);
      Services.EventAggregator.Publish(new Events.NavigatedEventMessage(navInfo));
    }
    public static void NavigationFailed<T>(Guid navigationId) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(navigationId);
      Services.EventAggregator.Publish(new Events.NavigationFailedEventMessage(navInfo));
    }

    private static NavigationInfo CreateNavigationInfo<T>(Guid navigationId) where T : IViewModelBase
    {
      if (typeof(T).Name == typeof(IViewModelBase).Name)
        throw new ArgumentException(
          "Generic type must be an instance of a ViewModel that implements IViewModelBase, not be IViewModelBase itself.");

      var viewModelCoreNoSpaces = ViewModels.ViewModelBase.GetCoreViewModelName(typeof(T));
      var navInfo = new NavigationInfo(navigationId, viewModelCoreNoSpaces);
      return navInfo;
    }


    public static void AuthenticationChanged()
    {
      Services.EventAggregator.Publish(new AuthenticationChangedEventMessage());
    }

  }
}
