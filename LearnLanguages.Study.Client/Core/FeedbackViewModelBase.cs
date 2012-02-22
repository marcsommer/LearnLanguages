using System;
using System.Net;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  public abstract class FeedbackViewModelBase : ViewModelBase, IFeedbackViewModelBase
  {
    public abstract IFeedback GetFeedback(int timeoutMilliseconds);
    public abstract void GetFeedbackAsync(int timeoutMilliseconds, 
                                          AsyncCallback<IFeedback> callback);

    private IFeedback _Feedback;
    public IFeedback Feedback
    {
      get { return _Feedback; }
      set
      {
        if (value != _Feedback)
        {
          _Feedback = value;
          NotifyOfPropertyChange(() => Feedback);
        }
      }
    }

    private bool _IsEnabled;
    public bool IsEnabled
    {
      get { return _IsEnabled; }
      set
      {
        if (value != _IsEnabled)
        {
          _IsEnabled = value;
          NotifyOfPropertyChange(() => IsEnabled);
        }
      }
    }
  }
}
