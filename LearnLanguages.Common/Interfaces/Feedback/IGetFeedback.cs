using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IGetFeedback
  {
    IFeedback GetFeedback(int timeoutMilliseconds);
    void GetFeedbackAsync(int timeoutMilliseconds, Delegates.AsyncCallback<IFeedback> callback);
  }
}
