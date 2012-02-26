using System;
using System.Net;
using LearnLanguages.Common.ViewModelBases;

namespace LearnLanguages.Study
{
  public abstract class StudyItemViewModelBase : ViewModelBase, Interfaces.IStudyItemViewModelBase
  {
    public abstract void Show(Common.Delegates.ExceptionCheckCallback callback);
    public abstract void Abort();

    protected DateTime _DateTimeQuestionShown { get; set; }
    protected DateTime _DateTimeAnswerShown { get; set; }

    public Guid ReviewMethodId
    {
      get { return GetReviewMethodId(); }
    }
    protected abstract Guid GetReviewMethodId();
  }
}
