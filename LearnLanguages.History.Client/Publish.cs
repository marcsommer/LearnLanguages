using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace LearnLanguages.History
{
  public class Publish : Common.Interfaces.IHistoryPublisher
  {
    public Publish()
    {
      EventAggregator = Services.EventAggregator;
    }

    #region Singleton Pattern Members
    private static volatile Publish _The;
    private static object _Lock = new object();
    /// <summary>
    /// Singleton instance.  Named "The" for esthetic reasons.
    /// </summary>
    public static Publish The
    {
      get
      {
        if (_The == null)
        {
          lock (_Lock)
          {
            if (_The == null)
              _The = new Publish();
          }
        }

        return _The;
      }
    }
    #endregion

    [Import]
    public IEventAggregator EventAggregator { get; set; }

    private object _PublishLock = new object();

    public void HistoryEvent(Common.Interfaces.IHistoryEvent historyEvent)
    {
      lock (_PublishLock)
      {
        EventAggregator.Publish(historyEvent);
      }
    }
  }
}
