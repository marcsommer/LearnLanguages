using LearnLanguages.Common.Interfaces;
using System;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudier<TJob, TTarget> : IDoAJob<TJob>
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
  }
}
