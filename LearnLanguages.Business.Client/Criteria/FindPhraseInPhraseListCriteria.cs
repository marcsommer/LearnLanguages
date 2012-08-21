
using System;
using System.Net;
using Csla.Serialization;
using Csla;
using System.Collections.Generic;
using Csla.Core;

namespace LearnLanguages.Business.Criteria
{
  /// <summary>
  /// Use this when searching for a phrase text/language text in a given PhraseList list.
  /// </summary>
  [Serializable]
  public class FindPhraseInPhraseListCriteria : CriteriaBase<FindPhraseInPhraseListCriteria>
  {
    /// <summary>
    /// DO NOT USE.  FOR SERIALIZATION ONLY.
    /// </summary>
    public FindPhraseInPhraseListCriteria()
    {
      //ELIDED
    }

    public FindPhraseInPhraseListCriteria(string phraseText, string languageText, PhraseList phrases)
    {
      PhraseText = phraseText;
      LanguageText = languageText;
      Phrases = phrases;
    }
        
    public static readonly PropertyInfo<PhraseList> PhrasesProperty = 
      RegisterProperty<PhraseList>(c => c.Phrases);
    public PhraseList Phrases
    {
      get { return ReadProperty(PhrasesProperty); }
      private set { LoadProperty(PhrasesProperty, value); }
    }

    public static readonly PropertyInfo<string> PhraseTextProperty =
      RegisterProperty<string>(c => c.PhraseText);
    public string PhraseText
    {
      get { return ReadProperty(PhraseTextProperty); }
      private set { LoadProperty(PhraseTextProperty, value); }
    }

    public static readonly PropertyInfo<string> LanguageTextProperty =
      RegisterProperty<string>(c => c.LanguageText);
    public string LanguageText
    {
      get { return ReadProperty(LanguageTextProperty); }
      private set { LoadProperty(LanguageTextProperty, value); }
    }
  }
}
