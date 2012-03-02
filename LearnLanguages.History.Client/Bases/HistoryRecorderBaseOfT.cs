using System;
using Caliburn.Micro;

namespace LearnLanguages.History.Bases
{
  /// <summary>
  /// Records history events if should handle message.
  /// </summary>
  public abstract class HistoryRecorderBase<T> : IHandle<T>
  {
    protected bool _IsEnabled = false;
    /// <summary>
    /// Changing this affects whether this subscribes to HistoryPublisher events.
    /// </summary>
    public bool IsEnabled
    {
      get
      {
        return _IsEnabled;
      }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;
          if (_IsEnabled)
            HistoryPublisher.Ton.SubscribeToEvents(this);
          else
            HistoryPublisher.Ton.UnsubscribeFromEvents(this);
        }
      }
    }

    public void Handle(T message)
    {
      if (ShouldRecord(message))
        Record(message);
    }

    protected abstract bool ShouldRecord(T message);
    protected abstract void Record(T message);
  }
}
