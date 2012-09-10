using System;
using System.Net;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new translation, loads it with given phrases.  
  /// </summary>
  [Serializable]
  public class TranslationCreator : Common.CslaBases.ReadOnlyBase<TranslationCreator>
  {

    #region Factory Methods

    /// <summary>
    /// Creates a new translation, making child-copies of the phrasesCriteria.Phrases and adding these
    /// children to translation.Phrases.
    /// </summary>
    /// <param name="phrasesCriteria">collection of PhraseEdits, do not have to be marked as children.</param>
    /// <param name="callback">callback executed once translation is completely populated</param>
    public static void CreateNew(Criteria.ListOfPhrasesCriteria phrasesCriteria, EventHandler<DataPortalResult<TranslationCreator>> callback)
    {
      DataPortal.BeginCreate<TranslationCreator>(phrasesCriteria, callback);
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<TranslationEdit> TranslationProperty = RegisterProperty<TranslationEdit>(c => c.Translation);
    public TranslationEdit Translation
    {
      get { return GetProperty(TranslationProperty); }
      private set { LoadProperty(TranslationProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(Criteria.ListOfPhrasesCriteria phrasesCriteria)
    {
      //INITIALIZE TRANSLATION CREATOR
      RetrieverId = Guid.NewGuid();
      Translation = TranslationEdit.NewTranslationEdit();

      //WILL USE THIS TO POPULATE THE TRANSLATION
      List<PhraseEdit> phrasesToUse = new List<PhraseEdit>(phrasesCriteria.Phrases);
      
      //FILL TRANSLATION.PHRASES WITH EMPTY PHRASES
      for (int i = 0; i < phrasesToUse.Count; i++)
      {
        Translation.Phrases.AddNew();
      }
      
      //RETRIEVE ANY ALREADY-EXISTING PHRASES IN DB
      var retriever = PhrasesByTextAndLanguageRetriever.CreateNew(phrasesCriteria);
      for (int i = 0; i < phrasesToUse.Count; i++)
      {
        var phraseToAdd = phrasesToUse[i];

        //IF THE RETRIEVEDPHRASES CONTAINS TO THE KEY (WHICH IT SHOULD) 
        //AND THE RETRIEVER FOUND AN ALREADY EXISTING PHRASE IN DB,
        //THEN REPLACE PHRASETOADD WITH THAT DB PHRASE
        if (retriever.RetrievedPhrases.ContainsKey(phraseToAdd.Id) &&
            retriever.RetrievedPhrases[phraseToAdd.Id] != null)
          phraseToAdd = retriever.RetrievedPhrases[phraseToAdd.Id];

        var translationChildPhrase = Translation.Phrases[i];
        var dto = phraseToAdd.CreateDto();
        translationChildPhrase.LoadFromDtoBypassPropertyChecks(dto);
      }
    }

#endif

    #endregion

  }
}
