using System;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;
using System.ComponentModel.Composition.Hosting;

namespace LearnLanguages
{
  public static class Services
  {
    public static void Initialize(CompositionContainer container)
    {
      _Container = container;
    }

    private static CompositionContainer _Container;
    public static CompositionContainer Container { get { return _Container; } }

    private static IEventAggregator _EventAggregator;
    public static IEventAggregator EventAggregator
    {
      get
      {
        if (_Container == null)
          throw new NullReferenceException(CommonResources.ErrorMsgNotInjected("Container"));
        if (_EventAggregator == null)
          _EventAggregator = Container.GetExportedValue<IEventAggregator>();

        return _EventAggregator;
      }
    }

    private static IWindowManager _WindowManager;
    public static IWindowManager WindowManager
    {
      get
      {
        if (_Container == null)
          throw new NullReferenceException(CommonResources.ErrorMsgNotInjected("Container"));
        if (_WindowManager == null)
          _WindowManager = Container.GetExportedValue<IWindowManager>();

        return _WindowManager;
      }
    }

    private static ILogger _Logger;
    public static ILogger Logger
    {
      get
      {
        if (_Container == null)
          throw new NullReferenceException("Container has not been initialized");
        if (_Logger == null)
          _Logger = Container.GetExportedValue<ILogger>();
        return _Logger;
      }
    }

    public static void Log(string msg, LogPriority priority, LogCategory category)
    {
      if (Logger != null)
        Logger.Log(msg, priority, category);
    }

    //private static NavigationController _Navigator;
    //public static NavigationController Navigator
    //{
    //  get
    //  {
    //    if (_Navigator == null)
    //      _Navigator = Container.Resolve<NavigationController>();
    //    return _Navigator;
    //  }
    //}

    
  }
}
