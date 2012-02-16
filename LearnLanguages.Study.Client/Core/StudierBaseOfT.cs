using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Offer;
using Caliburn.Micro;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<J, T> : IDo<J, T>//,
                                            //IHandle<Opportunity<T>>
    where J : IJobInfo<T>
  {
    public StudierBase()
    {
      HasDoneThisBefore = false;
      Id = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }
    protected J _StudyJobInfo { get; set; }

    public void Do(J studyJobInfo)
    {
      if (studyJobInfo == null)
        throw new ArgumentNullException("target");

      _StudyJobInfo = studyJobInfo;

      DoImpl();
      HasDoneThisBefore = true;
    }

    public bool HasDoneThisBefore { get; protected set; }

    /// <summary>
    /// Should take into account if HasStudied before.
    /// </summary>
    protected abstract void DoImpl();


    public void PleaseStopDoing(Guid jobInfoId)
    {
      //if (_StudyJobInfo.Id == jobInfoId)
      //  AbortJob();
    }

    //public abstract void Handle(Opportunity<T> message);
  }
}
