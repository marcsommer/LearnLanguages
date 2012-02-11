namespace LearnLanguages.Common.Interfaces
{
  public interface IHistoryPublisher
  {
    void HistoryEvent(IHistoryEvent historyEvent);
  }
}
