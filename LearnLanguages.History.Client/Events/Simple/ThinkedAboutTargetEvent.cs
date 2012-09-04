using System;
namespace LearnLanguages.History.Events
{
  public class ThinkedAboutTargetEvent : Bases.ThinkingEventBase
  {

    public ThinkedAboutTargetEvent(Guid thinkId)
      : base(thinkId)
    {
    }

    public Guid TargetId
    {
      get
      {
        return GetDetail<Guid>(HistoryResources.Key_TargetId);
      }
    }

    public static void Publish(Guid targetId)
    {
      History.HistoryPublisher.Ton.PublishEvent(new ThinkedAboutTargetEvent(targetId));
    }
  }
}
