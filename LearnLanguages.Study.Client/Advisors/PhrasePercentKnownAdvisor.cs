using System;
using LearnLanguages.Common;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Business;
using Caliburn.Micro;
using Csla.Core;

namespace LearnLanguages.Study.Advisors
{
  /// <summary>
  /// This advisor expects a question StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown with 
  /// State == PhraseEdit, answer is type double between 0.0 and 1.0.
  /// </summary>
  public class PhrasePercentKnownAdvisor : IAdvisor, IHandle<History.Events.ReviewedLineEvent>
  {
    #region Singleton Pattern Members
    private static volatile PhrasePercentKnownAdvisor _Ton;
    private static object _Lock = new object();
    public static PhrasePercentKnownAdvisor Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new PhrasePercentKnownAdvisor();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public Guid Id { get { return Guid.Parse(StudyResources.AdvisorIdPhrasePercentKnownStudyAdvisor); } }
    public bool AskAdvice(QuestionArgs questionArgs, AsyncCallback<object> answerCallback)
    {
      if (!(questionArgs.State is PhraseEdit) ||
            questionArgs.Question != StudyResources.AdvisorQuestionWhatIsPhrasePercentKnown)
        return false;

      ///TODO: COME UP WITH ANSWER
      ///1) First, check cache.  This will be the most up-to-date that we care about.
      ///2) Get all history events related to this phrase.  This includes:
      ///   -if this phrase exists in db, check for history.  If have history, then use that.  otherwise,
      ///   -check for a phrase that contains this phrase
      ///   -composition of individual words
      ///2) Analyze history events to project how much we think the user knows for this phrase.

      return true;
    }

    public void Handle(History.Events.ReviewedLineEvent message)
    {
      throw new NotImplementedException();
    }

    private MobileDictionary<PhraseEdit, double> Cache { get; set; }
  }
}
