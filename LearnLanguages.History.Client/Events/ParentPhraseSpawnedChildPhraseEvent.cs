﻿using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Events
{
  [Serializable]
  public class ParentPhraseSpawnedChildPhraseEvent : Bases.ParentPhraseChildPhraseEventBase
  {
    public ParentPhraseSpawnedChildPhraseEvent(PhraseEdit parent, PhraseEdit child)
      : base(parent, child)
    {
    }

    public static void Pub(PhraseEdit parent, PhraseEdit child)
    {
      Publish.The.HistoryEvent(new ParentPhraseSpawnedChildPhraseEvent(parent, child));
    }
  }
}