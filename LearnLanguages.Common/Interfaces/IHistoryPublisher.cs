namespace LearnLanguages.Common.Interfaces
{
  public interface IHistoryPublisher
  {
    void PublishEvent(IHistoryEvent historyEvent);
  }
}
