using Csla.Serialization;
using LearnLanguages.Business;
using System;
using System.Collections.Generic;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class PhraseViewedOnScreenEvent : Bases.SinglePhraseEventBase
  {
    public PhraseViewedOnScreenEvent()
      : base()
    {

    }

    public PhraseViewedOnScreenEvent(PhraseEdit phrase)
      : base(phrase)
    {

    }

    public PhraseViewedOnScreenEvent(PhraseEdit phrase, TimeSpan duration)
      : base(phrase, duration)
    {

    }
  }
}
