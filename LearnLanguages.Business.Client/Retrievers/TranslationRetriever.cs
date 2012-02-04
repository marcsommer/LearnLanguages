using System;
using System.Net;
using Csla;
using Csla.Serialization;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new translation, loads it with given phrases.  
  /// </summary>
  [Serializable]
  public class TranslationRetriever : Common.CslaBases.ReadOnlyBase<TranslationRetriever>
  {
    #region Factory Methods

    public static void CreateNew(PhraseCollectionCriteria phrases, EventHandler<DataPortalResult<TranslationRetriever>> callback)
    {
      DataPortal.BeginCreate<TranslationRetriever>(phrases, callback);
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
    public void DataPortal_Create(PhraseCollectionCriteria phrasesCriteria)
    {
      foreach (var phrase in phrasesCriteria.Phrases)
      {
        phrase.Save()
      }
      RetrieverId = Guid.NewGuid();

      Translation = TranslationEdit.NewTranslationEdit();
      
      var phraseA = Translation.Phrases.AddNew();
      
      phraseA.LanguageId = LanguageEdit.GetDefaultLanguageId();
      //phraseA.Language = LanguageEdit.GetLanguageEdit(phraseA.LanguageId);
      phraseA.Language = DataPortal.FetchChild<LanguageEdit>(phraseA.LanguageId);
      
      var phraseB = Translation.Phrases.AddNew();
      phraseB.LanguageId = LanguageEdit.GetDefaultLanguageId();
      //phraseB.Language = LanguageEdit.GetLanguageEdit(phraseB.LanguageId);
      phraseB.Language = DataPortal.FetchChild<LanguageEdit>(phraseB.LanguageId);
    }
#endif

    #endregion

  }
}
