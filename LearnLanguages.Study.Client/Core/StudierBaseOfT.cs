using System;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<J, T> : IDo<J, T>
    where J : IJobInfo<T>
  {
    public StudierBase()
    {
      IsNotFirstRun = false;
      Id = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }
   
    protected J _StudyJobInfo { get; set; }
    /// <summary>
    /// Todo: change IsNotFirstRun bad name, need to refactor to IsFirstRun and change logic.
    /// </summary>
    public bool IsNotFirstRun { get; protected set; }
    protected object _AbortLock = new object();
    protected bool _AbortHasBeenFlagged
    {
      get
      {
        lock (_AbortLock)
        {
          return _AbortHasBeenFlagged;
        }
      }
      set
      {
        lock (_AbortLock)
        {
          _AbortHasBeenFlagged = value;
        }
      }
    }

    public void Do(J studyJobInfo)
    {
      if (studyJobInfo == null)
        throw new ArgumentNullException("target");

      _StudyJobInfo = studyJobInfo;

      DoImpl();
      IsNotFirstRun = true;
    }
    /// <summary>
    /// Should take into account if HasDoneThisBefore.
    /// </summary>
    protected abstract void DoImpl();
    public virtual void PleaseStopDoing(Guid jobInfoId)
    {
      if (_StudyJobInfo.Id == jobInfoId)
        _AbortHasBeenFlagged = true;
    }
  }
}
