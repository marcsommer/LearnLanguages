
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseCriteria : CriteriaBase<PhraseCriteria>
  {
    public PhraseCriteria(PhraseEdit phrase)
    {
      Phrase = phrase;
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseProperty = RegisterProperty<PhraseEdit>(c => c.Phrase);
    public PhraseEdit Phrase
    {
      get { return ReadProperty(PhraseProperty); }
      private set { LoadProperty(PhraseProperty, value); }
    }
  }
}
