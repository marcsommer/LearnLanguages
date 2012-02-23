
using System;
using System.Net;
using Csla.Serialization;
using Csla;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// Use this criteria for conduction translation searches.
  /// </summary>
  [Serializable]
  public class TranslationSearchCriteria : CriteriaBase<TranslationSearchCriteria>
  {
    /// <summary>
    /// DON'T USE THIS CTOR.  THIS IS A READ ONLY CRITERIA CLASS.  THIS CTOR IS ONLY HERE
    /// BECAUSE SERIALIZATION REQUIRES IT.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public TranslationSearchCriteria()
    {
      //required for serialization
    }
    public TranslationSearchCriteria(PhraseEdit phrase, string targetLanguageText, string contextText = "")
    {
      Phrase = phrase;
      TargetLanguageText = targetLanguageText;
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseProperty = RegisterProperty<PhraseEdit>(c => c.Phrase);
    public PhraseEdit Phrase
    {
      get { return ReadProperty(PhraseProperty); }
      private set { LoadProperty(PhraseProperty, value); }
    }

    public static readonly PropertyInfo<string> TargetLanguageTextProperty = 
      RegisterProperty<string>(c => c.TargetLanguageText);
    public string TargetLanguageText
    {
      get { return ReadProperty(TargetLanguageTextProperty); }
      private set { LoadProperty(TargetLanguageTextProperty, value); }
    }

    public static readonly PropertyInfo<string> ContextTextProperty =
      RegisterProperty<string>(c => c.ContextText);
    public string ContextText
    {
      get { return ReadProperty(ContextTextProperty); }
      private set { LoadProperty(ContextTextProperty, value); }
    }
  }
}
