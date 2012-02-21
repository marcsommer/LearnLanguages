using System;
using System.Net;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public abstract class FeedbackViewModelBase : ViewModelBase, IFeedbackViewModelBase
  {
    public abstract void Show(Common.Delegates.ExceptionCheckCallback callback);
    public abstract void Abort();

    public abstract IFeedback GetFeedback();
    public abstract void GetFeedbackAsync(Common.Delegates.AsyncCallback<Common.ResultArgs<IFeedback>> callback);

    public IFeedback Feedback { get; }
    public bool IsEnabled { get; set; }
  }
}
