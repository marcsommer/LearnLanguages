using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class LineReviewedEvent : Bases.SingleLineEventBase
  {
    public LineReviewedEvent()
      : base()
    {

    }

    public LineReviewedEvent(Guid lineId, Guid reviewMethodId, string lineText, int lineNumber, Guid phraseId,
      Guid languageId, string languageText, double feedbackAsDouble, TimeSpan duration)
      : base(lineId, lineText, lineNumber, phraseId, languageId, languageText, duration)
    {
      AddDetail(HistoryResources.Key_ReviewMethodId, reviewMethodId);
      AddDetail(HistoryResources.Key_FeedbackAsDouble, feedbackAsDouble);
    }


    //public LineReviewedEvent(LineEdit line, Guid reviewMethodId)
    //  : base(line, 
    //         new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId))
    //{

    //}

    //public LineReviewedEvent(LineEdit line, Guid reviewMethodId, TimeSpan duration)
    //  : base(line, 
    //         duration,
    //         new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId))
    //{

    //}

    //public LineReviewedEvent(LineEdit line, Guid reviewMethodId, TimeSpan duration, double feedbackAsDouble)
    //  : base(line,
    //         duration,
    //         new KeyValuePair<string, object>(HistoryResources.Key_ReviewMethodId, reviewMethodId),
    //         new KeyValuePair<string, object>(HistoryResources.Key_FeedbackAsDouble, feedbackAsDouble))
    //{

    //}
  }
}
