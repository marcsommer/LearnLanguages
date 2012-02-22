using System;
using System.Net;
using LearnLanguages.Common.Delegates;
using Caliburn.Micro;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public class NextStudyItemIterator<T> : IResult
  {
    public NextStudyItemIterator(StudierBase<T> studier, T target)
    {
      Studier = studier;
      Target = target;
    }

    public StudierBase<T> Studier { get; set; }
    public T Target { get; set; }
    public IViewModelBase Product { get; set; }

    public event EventHandler<ResultCompletionEventArgs> Completed;

    public void Execute(ActionExecutionContext context)
    {
      Studier.GetNextStudyItemViewModel((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          Product = r.Object.ViewModel;

          Completed(this, new ResultCompletionEventArgs());
        });
    }
  }
}
