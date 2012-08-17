﻿using System;
using System.Linq;
using System.Collections.Generic;

using Csla;
using Csla.Serialization;
using LearnLanguages.DataAccess;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class searches for a given phrase.  If it finds the translation that contains exactly 
  /// this phrase, then it returns with the translation.  If it finds no translation, then it returns
  /// null as translation.
  /// This does NOT search through all translations looking for containment of the phrase.  So, if you 
  /// search for "the", it will look for the translation that contains the phrase.Text=="the".  It will
  /// not return a translation such that it has a Phrase.Text="the quick brown fox".
  /// Right now, this is very slow.  It downloads a list of all phrases, and searches through them manually.
  /// It also does some other non-optimal things.
  /// </summary>
  [Serializable]
  public class TranslationSearchRetriever : Common.CslaBases.ReadOnlyBase<TranslationSearchRetriever>
  {
    #region Factory Methods

    /// <summary>
    /// Creates a new translation, making child-copies of the phrasesCriteria.Phrases and adding these
    /// children to translation.Phrases.
    /// </summary>
    /// <param name="phrasesCriteria">collection of PhraseEdits, do not have to be marked as children.</param>
    /// <param name="callback">callback executed once translation is completely populated</param>
    public static void CreateNew(Criteria.TranslationSearchCriteria criteria,
      EventHandler<DataPortalResult<TranslationSearchRetriever>> callback)
    {
      if (criteria == null)
        throw new ArgumentNullException("criteria");
      if (criteria.Phrase == null)
        throw new ArgumentException("criteria.Phrase == null");
      if (string.IsNullOrEmpty(criteria.TargetLanguageText))
        throw new ArgumentException("criteria.TargetLanguageText is null or empty");

      DataPortal.BeginCreate<TranslationSearchRetriever>(criteria, callback);
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
    public void DataPortal_Create(Criteria.TranslationSearchCriteria criteria)
    {
      RetrieverId = Guid.NewGuid();
      //Translation = TranslationEdit.NewTranslationEdit();

      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        Result<ICollection<PhraseDto>> result = phraseDal.GetAll();
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }

        var allPhraseDtos = result.Obj;


        //WE HAVE A LIST OF ALL THE PHRASES.  NOW SEARCH THROUGH FOR OUR PHRASE.
        var foundPhraseDto = (from phraseDto in allPhraseDtos
                           where phraseDto.Text == criteria.Phrase.Text &&
                                 phraseDto.LanguageId == criteria.Phrase.LanguageId
                           select phraseDto).FirstOrDefault();

        //ASSUME NO TRANSLATION
        Translation = null;

        //IF WE HAVEN'T FOUND A PHRASE, THEN WE WON'T FIND A TRANSLATION.
        if (foundPhraseDto == null)
          return;

        //WE FOUND A PHRASE, BUT WE STILL NEED TO LOOK FOR TRANSLATION FOR THAT PHRASE.
        var phraseEdit = DataPortal.Fetch<PhraseEdit>(foundPhraseDto.Id);
        var translationsContainingPhrase = TranslationList.GetAllTranslationsContainingPhraseById(phraseEdit);

        if (translationsContainingPhrase.Count == 0)
          return;

        //WE FOUND TRANSLATIONS WITH THAT PHRASE, BUT WE NEED THE ONES IN THE TARGET LANGUAGE ONLY
        var translationsInTargetLanguage = (from translation in translationsContainingPhrase
                                            where (from phrase in translation.Phrases
                                                   where phrase.Language.Text == criteria.TargetLanguageText
                                                   select phrase).Count() > 0
                                            select translation);
        if (translationsInTargetLanguage.Count() == 0)
          return;


        //WE FOUND TRANSLATIONS IN THE TARGET LANGUAGE, AND WE MUST NOW CHECK AGAINST CONTEXT (IF PROVIDED)
        if (!string.IsNullOrEmpty(criteria.ContextText))
        {
          //CONTEXT TEXT HAS BEEN PROVIDED
          Translation = (from t in translationsInTargetLanguage
                         where t.ContextPhrase != null &&
                               t.ContextPhrase.Text == criteria.ContextText
                         select t).FirstOrDefault();
        }
        else
        {
          //CONTEXT TEXT HAS NOT BEEN PROVIDED
          //IF WE FOUND ONE, THIS SETS TRANSLATION TO THE FIRST TRANSLATION FOUND.  
          Translation = translationsInTargetLanguage.First();
        }
      }
    }

#endif

    #endregion
  }
}
