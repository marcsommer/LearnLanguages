using LearnLanguages.Common.Interfaces;
using System;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudier<TJob, TTarget> 
    where TJob : IStudyJobInfo<TTarget>
  {
    /// <summary>
    /// Id unique to this studier's configuration.  For now, this is just a random Guid.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Performs a study iteration using the job info and the offerExchange given.
    /// </summary>
    void Study(TJob studyJobInfo, IExchange offerExchange);

    /// <summary>
    /// If this studier cannot study again, if either because it has no valid studyJobInfo/offerExchange,
    /// it has no further information to study, or for some other reason, this returns false.
    /// Otherwise, it returns true and the study iteration is performed, using the target and 
    /// offerExchange in the prerequisite Study(target, offerExchange) method.
    /// </summary>
    bool StudyAgain();
  }
}
