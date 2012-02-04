
using System;
using System.Net;
using System.Windows;
using Csla.Serialization;
using Csla;
using System.Collections.Generic;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseCollectionCriteria : CriteriaBase<PhraseCollectionCriteria>
  {
    public PhraseCollectionCriteria(params PhraseEdit[] phrases)
    {
      Phrases = new List<PhraseEdit>(phrases);
    }

    public static readonly PropertyInfo<ICollection<PhraseEdit>> PhrasesProperty = 
      RegisterProperty<ICollection<PhraseEdit>>(c => c.Phrases);
    public ICollection<PhraseEdit> Phrases
    {
      get { return ReadProperty(PhrasesProperty); }
      private set { LoadProperty(PhrasesProperty, value); }
    }
  }
}
