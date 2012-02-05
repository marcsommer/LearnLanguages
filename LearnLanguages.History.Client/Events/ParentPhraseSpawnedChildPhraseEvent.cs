using LearnLanguages.Business;
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
  }
}
