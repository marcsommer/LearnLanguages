using System;
using Caliburn.Micro;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public class InitializeDefaultStudyPartner : IResult
  {
    public InitializeDefaultStudyPartner(DefaultMultiLineTextsStudier studier, MultiLineTextList target)
    {
      Studier = studier;
      Target = target;
    }

    public DefaultMultiLineTextsStudier Studier { get; set; }
    public MultiLineTextList Target { get; set; }

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
