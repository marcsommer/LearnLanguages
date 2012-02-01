using Caliburn.Micro;
using System.Diagnostics;
//using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Navigation.Interfaces;

namespace LearnLanguages.Silverlight
{
  public class DebugEventMessageListener : IHandle<INavigationEventMessage>
  {
    public DebugEventMessageListener()
    {
      Services.EventAggregator.Subscribe(this);
    }

    public void Handle(INavigationEventMessage message)
    {
      //Debug.WriteLine("NavigationEventMessage Start");
      Debug.WriteLine(message.GetType().Name + " Start");
      Debug.WriteLine("NavigationId: " + message.NavigationInfo.NavigationId.ToString());
      Debug.WriteLine(message.NavigationInfo.ViewModelCoreNoSpaces);
      Debug.WriteLine(message.NavigationInfo.Uri.ToString());
      Debug.WriteLine("\r\n");
    }
  }
}
