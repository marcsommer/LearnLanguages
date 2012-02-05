using System;
using System.Collections.Generic;
using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class SinglePhraseEventBase : HistoryEventBase
  {
    public SinglePhraseEventBase()
      : base()
    {

    }

    public SinglePhraseEventBase(PhraseEdit phrase)
      : base(TimeSpan.MinValue,
             new KeyValuePair<string, object>(HistoryResources.Key_PhraseId, phrase.Id)) 
    {

    }

    public SinglePhraseEventBase(PhraseEdit phrase, TimeSpan duration)
      : base(duration,
             new KeyValuePair<string, object>(HistoryResources.Key_PhraseId, phrase.Id))
    {

    }

    public SinglePhraseEventBase(PhraseEdit phrase, params KeyValuePair<string, object>[] details)
      : base(TimeSpan.MinValue,
             new KeyValuePair<string, object>(HistoryResources.Key_PhraseId, phrase.Id))
    {
      AddDetails(details);
    }

    public SinglePhraseEventBase(PhraseEdit phrase, TimeSpan duration, params KeyValuePair<string, object>[] details)
      : base(duration,
             new KeyValuePair<string, object>(HistoryResources.Key_PhraseId, phrase.Id))
    {
      AddDetails(details);
    }
  }
}
