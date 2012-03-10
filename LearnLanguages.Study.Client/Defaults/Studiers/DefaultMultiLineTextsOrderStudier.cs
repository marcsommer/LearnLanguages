using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Navigation.EventMessages;
using Caliburn.Micro;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  public class DefaultMultiLineTextsOrderStudier : 
    StudierBase<StudyJobInfo<MultiLineTextList, IViewModelBase>, MultiLineTextList, IViewModelBase>,
    IHandle<NavigationRequestedEventMessage>
  {
    public DefaultMultiLineTextsOrderStudier()
    {
      Services.EventAggregator.Subscribe(this);//navigation
    }
    protected override void DoImpl()
    {
      throw new NotImplementedException();
    }

    public void Handle(NavigationRequestedEventMessage message)
    {
      AbortStudying();
    }

    private void AbortStudying()
    {
      _AbortIsFlagged = true;
    }

    private object _AbortLock = new object();
    private bool _abortIsFlagged = false;
    private bool _AbortIsFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _abortIsFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _abortIsFlagged = value;
        }
      }
    }

  }
}
