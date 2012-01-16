using LearnLanguages.Silverlight.Interfaces;

namespace LearnLanguages.Silverlight.Events
{
  public abstract class NavigationEventMessage : Interfaces.INavigationEventMessage
  {
    public NavigationEventMessage(NavigationInfo navigationInfo)
    {
      NavigationInfo = navigationInfo;
    }

    public NavigationInfo NavigationInfo { get; private set; }
  }
}
