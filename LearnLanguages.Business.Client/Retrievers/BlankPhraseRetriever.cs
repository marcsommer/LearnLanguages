using System;
using System.Net;
using Csla;
using Csla.Serialization;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class creates a new phrase, loads it with the default language.
  /// </summary>
  [Serializable]
  public class BlankPhraseRetriever : Common.CslaBases.ReadOnlyBase<BlankPhraseRetriever>
  {
    #region Factory Methods

    public static void CreateNew(EventHandler<DataPortalResult<BlankPhraseRetriever>> callback)
    {
      DataPortal.BeginCreate<BlankPhraseRetriever>(callback);
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<PhraseEdit> PhraseProperty = RegisterProperty<PhraseEdit>(c => c.Phrase);
    public PhraseEdit Phrase
    {
      get { return GetProperty(PhraseProperty); }
      private set { LoadProperty(PhraseProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create()
    {
      RetrieverId = Guid.NewGuid();

      Phrase = PhraseEdit.NewPhraseEdit();
      
      Phrase.LanguageId = LanguageEdit.GetDefaultLanguageId();
      Phrase.Language = DataPortal.FetchChild<LanguageEdit>(Phrase.LanguageId);
        //LanguageEdit.GetLanguageEdit(Phrase.LanguageId);

      //var phraseB = Translation.Phrases.AddNew();
      //phraseB.LanguageId = LanguageEdit.GetDefaultLanguageId();
      //phraseB.Language = LanguageEdit.GetLanguageEdit(phraseB.LanguageId);
    }
#endif

    #endregion
  }
}
