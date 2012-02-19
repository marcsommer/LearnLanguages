using System;
using System.Net;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<T>
  {
    public abstract void SetTarget(T target);
    public abstract void InitializeForNewStudySession(T target);
    public abstract void GetNextStudyItemViewModel(AsyncCallback<StudyItemViewModelArgs> callback);
  }
}
