using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IGetFeedback
  {
    IFeedback GetFeedback();
    void GetFeedbackAsync(Delegates.AsyncCallback<Args.ResultArgs<IFeedback>> callback);
  }
}
