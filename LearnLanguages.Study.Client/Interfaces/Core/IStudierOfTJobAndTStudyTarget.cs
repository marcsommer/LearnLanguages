using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudier<TJob, TStudyTarget> 
    where TJob : IStudyJobInfo<TStudyTarget>
  {
    /// <summary>
    /// Performs a study iteration using the job info and the offerExchange given.
    /// </summary>
    void Study(TJob studyJobInfo, IOfferExchange offerExchange);

    /// <summary>
    /// If this studier cannot study again, if either because it has no valid studyJobInfo/offerExchange,
    /// it has no further information to study, or for some other reason, this returns false.
    /// Otherwise, it returns true and the study iteration is performed, using the studyTarget and 
    /// offerExchange in the prerequisite Study(studyTarget, offerExchange) method.
    /// </summary>
    bool StudyAgain();
  }
}
