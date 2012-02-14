using System;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace LearnLanguages.History
{
  public class History : Common.Interfaces.IHistoryPublisher, IPartImportsSatisfiedNotification
  {
    public History()
    {
      EventAggregator = Services.EventAggregator;
    }

    public Guid HistoryPublisherId { get { return Guid.Parse(HistoryResources.HistoryPublisherId); } }

    #region Singleton Pattern Members
    private static volatile History _Ton;
    private static object _Lock = new object();
    /// <summary>
    /// Singleton instance.  Named "The" for esthetic reasons.
    /// </summary>
    public static History Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new History();
          }
        }

        return _Ton;
      }
    }
    #endregion

    [Import]
    public IEventAggregator EventAggregator { get; set; }

    private object _PublishLock = new object();

    public void PublishEvent(Common.Interfaces.IHistoryEvent historyEvent)
    {
      lock (_PublishLock)
      {
        EventAggregator.Publish(historyEvent);
      }
    }

    public void OnImportsSatisfied()
    {
      if (EventAggregator != null)
        EventAggregator.Subscribe(this);
    }
  }
}
