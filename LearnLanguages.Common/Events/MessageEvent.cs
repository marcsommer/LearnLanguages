using LearnLanguages.Common.MessageShower;

namespace LearnLanguages.Common.Events
{
  public class MessageEvent
  {
    public MessageEvent(string text, MessagePriority priority, MessageType type)
    {
      Text = text;
      Priority = priority;
      Type = type;
    }

    public string Text { get; private set; }
    public MessagePriority Priority { get; private set; }
    public MessageType Type { get; private set; }
  }
}
