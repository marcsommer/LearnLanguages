using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Offer;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<J, T> : IDo<J, T>
    where J : IStudyJobInfo<T>
  {
    public StudierBase()
    {
      HasStudied = false;
      Id = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }
    protected J _StudyJobInfo { get; set; }

    public void Do(J studyJobInfo)
    {
      if (studyJobInfo == null)
        throw new ArgumentNullException("target");

      //if our old study target isn't empty and the new study target isn't the same one
      //then we haven't studied this before.
      //or the same reasoning for offer exchange
      if (_StudyJobInfo != null && 
          studyJobInfo.Id != _StudyJobInfo.Id)
        HasStudied = false;

      _StudyJobInfo = studyJobInfo;

      DoImpl();
      HasStudied = true;
    }

    public bool HasStudied { get; protected set; }

    /// <summary>
    /// Should take into account if HasStudied before.
    /// </summary>
    protected abstract void DoImpl();
  }
}
