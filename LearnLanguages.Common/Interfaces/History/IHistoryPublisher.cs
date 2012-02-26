using System;
namespace LearnLanguages.Common.Interfaces
{
  public interface IHistoryPublisher
  {
    Guid Id { get; }
    void PublishEvent(IHistoryEvent historyEvent);
  }
}
