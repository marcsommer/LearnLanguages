using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudier<J, T> 
    where J : IStudyJobInfo<T>
  {
    /// <summary>
    /// Performs a study iteration using the job info and the offerExchange given.
    /// </summary>
    void Study(J studyJobInfo, IOfferExchange offerExchange);

    /// <summary>
    /// If this studier cannot study again, if either because it has no valid studyJobInfo/offerExchange,
    /// it has no further information to study, or for some other reason, this returns false.
    /// Otherwise, it returns true and the study iteration is performed, using the studyTarget and 
    /// offerExchange in the prerequisite Study(studyTarget, offerExchange) method.
    /// </summary>
    bool StudyAgain();

    /// <summary>
    /// Identifier for the studier's context for studying the study target.  Specifically with my 
    /// current design, this is how you tell if a studier studies "Meaning" or "Order".
    /// </summary>
    string GetStudyContext();
  }
}
