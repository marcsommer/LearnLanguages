using System;
using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Business;

namespace LearnLanguages.Study.Advisors
{
  public class PhrasePercentKnownAdvisor : IAdvisor
  {
    public Guid Id { get { return Guid.Parse(StudyResources.AdvisorIdPhrasePercentKnownStudyAdvisor); } }
    public bool AskAdvice(QuestionArgs question, AsyncCallback<object> answer)
    {
      if (!(question.State is PhraseEdit) ||
            question.Question != StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown)
        return false;

      ///TODO: COME UP WITH ANSWER
      ///1) Get all history events related to this phrase.  This includes:
      ///   -if this phrase exists in db, check for history.  If have history, then use that.  otherwise,
      ///   -check for a phrase that contains this phrase
      ///   -composition of individual words
      ///2) Analyze history events to project how much we think the user knows for this phrase.

      return true;
    }
  }
}
