using System;
using Csla.Serialization;
using LearnLanguages.Business;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ReviewedPhraseEvent : Bases.SinglePhraseEventBase
  {
    public ReviewedPhraseEvent()
      : base()
    {

    }

    public ReviewedPhraseEvent(PhraseEdit phrase, Guid reviewMethodId, TimeSpan duration)
      : base(phrase, duration)
    {
      AddReviewMethodId(reviewMethodId);
    }

    public ReviewedPhraseEvent(Guid phraseId, Guid languageId, Guid reviewMethodId, TimeSpan duration)
      : base(phraseId, languageId, duration)
    {
      AddReviewMethodId(reviewMethodId);
    }

    private void AddReviewMethodId(Guid reviewMethodId)
    {
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
    }
  }
}
