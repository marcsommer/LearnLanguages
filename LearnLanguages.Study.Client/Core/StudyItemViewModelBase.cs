using System;
using System.Net;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study
{
  public abstract class StudyItemViewModelBase : ViewModelBase, Interfaces.IStudyItemViewModelBase
  {
    public virtual void Show(Common.Delegates.ExceptionCheckCallback callback)
    {
      _Callback = callback;
      DispatchShown();
    }
    public abstract void Abort();

    protected DateTime _DateTimeQuestionShown { get; set; }
    protected DateTime _DateTimeAnswerShown { get; set; }
    protected ExceptionCheckCallback _Callback { get; set; }

    public Guid ReviewMethodId
    {
      get { return GetReviewMethodId(); }
    }
    protected abstract Guid GetReviewMethodId();

    protected void DispatchShown()
    {
      if (Shown != null)
        Shown(this, EventArgs.Empty);
    }
    
    public event EventHandler Shown;
  }
}
