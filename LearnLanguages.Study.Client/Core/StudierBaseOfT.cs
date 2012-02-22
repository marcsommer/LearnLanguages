using System;
using System.Net;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<T>
  {
    //public abstract void SetTarget(T target);
    protected T _Target { get; set; }
    public abstract void InitializeForNewStudySession(T target, ExceptionCheckCallback completedCallback);
    public abstract void GetNextStudyItemViewModel(AsyncCallback<StudyItemViewModelArgs> callback);
  }
}
