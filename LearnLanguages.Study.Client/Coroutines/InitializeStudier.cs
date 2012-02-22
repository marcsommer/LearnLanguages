using System;
using System.Net;
using LearnLanguages.Common.Delegates;
using Caliburn.Micro;

namespace LearnLanguages.Study
{
  public class InitializeStudier<T> : IResult
  {
    public InitializeStudier(StudierBase<T> studier, T target)
    {
      Studier = studier;
      Target = target;
    }

    public StudierBase<T> Studier { get; set; }
    public T Target { get; set; }

    public event EventHandler<ResultCompletionEventArgs> Completed;

    public void Execute(ActionExecutionContext context)
    {
      Studier.InitializeForNewStudySession(Target, (e) =>
        {
          if (e != null)
            throw e;
          Completed(this, new ResultCompletionEventArgs());
        });
    }
  }
}
