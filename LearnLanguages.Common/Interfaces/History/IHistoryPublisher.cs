using System;
namespace LearnLanguages.Common.Interfaces
{
  public interface IHistoryPublisher
  {
    Guid HistoryPublisherId { get; }
    void PublishEvent(IHistoryEvent historyEvent);
  }
}
