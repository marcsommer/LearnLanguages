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
      RetrieverId = Guid.NewGuid();
      Translation = TranslationEdit.NewTranslationEdit();
      
      //FILL TRANSLATION.PHRASES WITH EMPTY PHRASES
      for (int i = 0; i < phrasesCriteria.Phrases.Count; i++)
      {
        Translation.Phrases.AddNew();
      }

      for (int i = 0; i < phrasesCriteria.Phrases.Count; i++)
      {
        var sourcePhrase = phrasesCriteria.Phrases[i];
        var targetPhrase = Translation.Phrases[i];
        var dto = sourcePhrase.CreateDto();
        targetPhrase.LoadFromDtoBypassPropertyChecks(dto);
      }
    }

#endif

    #endregion

  }
}
