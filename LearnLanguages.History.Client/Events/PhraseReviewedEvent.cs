using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class PhraseReviewedEvent : Bases.SinglePhraseEventBase
  {
    public PhraseReviewedEvent()
      : base()
    {

    }

    public PhraseReviewedEvent(PhraseEdit phrase, Guid reviewMethodId)
      : base(phrase, 
             new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId))
    {

    }

    public PhraseReviewedEvent(PhraseEdit phrase, Guid reviewMethodId, TimeSpan duration)
      : base(phrase, 
             duration,
             new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId))
    {

    }
  }
}
