using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Navigation
{
  public static class Publish
  {
    public static void NavigationRequest<T>(string baseAddress) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(Guid.NewGuid(), baseAddress);
      Services.EventAggregator.Publish(new EventMessages.NavigationRequestedEventMessage(navInfo));
    }
    public static void Navigating<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
      Services.EventAggregator.Publish(new EventMessages.NavigatingEventMessage(navInfo));
    }
    public static void Navigated<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
      Services.EventAggregator.Publish(new EventMessages.NavigatedEventMessage(navInfo));
    }
    public static void NavigationFailed<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    {
      var navInfo = CreateNavigationInfo<T>(navigationId, baseAddress);
      Services.EventAggregator.Publish(new EventMessages.NavigationFailedEventMessage(navInfo));
    }

    private static NavigationInfo CreateNavigationInfo<T>(Guid navigationId, string baseAddress) where T : IViewModelBase
    {
      if (typeof(T).Name == typeof(IViewModelBase).Name)
        throw new ArgumentException(
          "Generic type must be an instance of a ViewModel that implements IViewModelBase, not be IViewModelBase itself.");

      var viewModelCoreNoSpaces = ViewModelBase.GetCoreViewModelName(typeof(T));
      var navInfo = new NavigationInfo(navigationId, viewModelCoreNoSpaces, baseAddress);
      return navInfo;
    }

    //public static void AuthenticationChanged()
    //{
    //  Services.EventAggregator.Publish(new AuthenticationChangedEventMessage());
    //}

    //public static void PartSatisfied(string part)
    //{
    //  Services.EventAggregator.Publish(new EventMessages.PartSatisfiedEventMessage(part));
    //}

  }
}
