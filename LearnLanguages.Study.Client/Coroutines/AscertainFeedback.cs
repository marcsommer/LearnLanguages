using System;
using System.Net;
using LearnLanguages.Common.Delegates;
using Caliburn.Micro;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public class AscertainFeedback : IResult
  {
    public AscertainFeedback(IFeedbackViewModelBase viewModel, int timeoutMilliseconds)
    {
      ViewModel = viewModel;
      TimeoutMilliseconds = timeoutMilliseconds;
    }

    public IFeedbackViewModelBase ViewModel { get; set; }
    public int TimeoutMilliseconds { get; set; }
    public IFeedback Feedback { get; private set; }

    public event EventHandler<ResultCompletionEventArgs> Completed;

    public void Execute(ActionExecutionContext context)
    {
      if (ViewModel == null)
        throw new Exception();

      ViewModel.IsEnabled = true;
      ViewModel.GetFeedbackAsync(TimeoutMilliseconds, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          Feedback = r.Object;
          ViewModel.IsEnabled = false;
          Completed(this, new ResultCompletionEventArgs());
        });
    }
  }
}
