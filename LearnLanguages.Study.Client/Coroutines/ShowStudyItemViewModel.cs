using System;
using System.Net;
using LearnLanguages.Common.Delegates;
using Caliburn.Micro;
using LearnLanguages.Study.Interfaces;

namespace LearnLanguages.Study
{
  public class ShowStudyItemViewModel : IResult
  {
    public ShowStudyItemViewModel(IStudyItemViewModelBase viewModel)
    {
      ViewModel = viewModel;
    }

    public IStudyItemViewModelBase ViewModel { get; set; }

    public event EventHandler<ResultCompletionEventArgs> Completed;

    public void Execute(ActionExecutionContext context)
    {
      if (ViewModel == null)
        throw new Exception();

      ViewModel.Show((e) =>
        {
          if (e != null)
            throw e;
          Completed(this, new ResultCompletionEventArgs());
        });
    }
  }
}
