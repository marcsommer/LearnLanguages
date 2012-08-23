using System;
namespace LearnLanguages.Silverlight.EventMessages
{
  public class ThinkingAboutSomethingEventMessage
  {

    public ThinkingAboutSomethingEventMessage(Guid thinkId)
    {
      ThinkId = thinkId;
    }

    public Guid ThinkId { get; set; }

    public static void Publish(Guid thinkId)
    {
      Services.EventAggregator.Publish(new ThinkingAboutSomethingEventMessage(thinkId));
    }
  }
}
