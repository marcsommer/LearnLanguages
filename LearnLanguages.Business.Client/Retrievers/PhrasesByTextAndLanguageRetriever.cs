using System;
using System.Linq;
using System.Net;
using Csla;
using Csla.Serialization;
using Csla.Core;
using LearnLanguages.DataAccess;
using System.Collections.Generic;

namespace LearnLanguages.Business
{
  /// <summary>
  /// This class retrieves phrases by searching using their text and language.text.
  /// </summary>
  [Serializable]
  public class PhrasesByTextAndLanguageRetriever : Common.CslaBases.ReadOnlyBase<PhrasesByTextAndLanguageRetriever>
  {
    #region Factory Methods

    /// <summary>
    /// Retrieves PhraseEdits from the DB that match the PhraseEdits given in the criteria.
    /// It stores these retrieved phrases in a dictionary, with the given phrase.Id in the criteria
    /// as the keys and the retrieved phrases as the values.
    /// 
    /// For any phrase in criteria given that is not found in DB, the retrieved phrase will be null.
    /// 
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">phrases to be retrieved from DB</param>
    /// <param name="callback">callback executed once retrieval is complete</param>
    public static void CreateNew(Criteria.ListOfPhrasesCriteria criteria, 
      EventHandler<DataPortalResult<PhrasesByTextAndLanguageRetriever>> callback)
    {
      DataPortal.BeginCreate<PhrasesByTextAndLanguageRetriever>(criteria, callback);
    }

#if !SILVERLIGHT
    /// <summary>
    /// Retrieves PhraseEdits from the DB that match the PhraseEdits given in the criteria.
    /// It stores these retrieved phrases in a dictionary, with the given phrase.Id in the criteria
    /// as the keys and the retrieved phrases as the values.
    /// 
    /// For any phrase in criteria given that is not found in DB, the retrieved phrase will be null.
    /// 
    /// THIS IS NOT OPTIMIZED, AND IT DOES MOST OF THE WORK ON THE SERVER SIDE.  I MIGHT SHOULD TURN THIS INTO A COMMAND OBJECT
    /// AND HAVE THIS DONE ON THE CLIENT SIDE, ESPECIALLY SINCE CURRENTLY IT IS JUST USING PHRASELIST.GETALL() AND THEN 
    /// WORKING OFF OF THAT ANYWAY.
    /// </summary>
    /// <param name="criteria">phrases to be retrieved from DB</param>
    public static PhrasesByTextAndLanguageRetriever CreateNew(Criteria.ListOfPhrasesCriteria criteria)
    {
      return DataPortal.Create<PhrasesByTextAndLanguageRetriever>(criteria);
    }
#endif

    #endregion

    #region Properties

    public static readonly PropertyInfo<Guid> RetrieverIdProperty = RegisterProperty<Guid>(c => c.RetrieverId);
    public Guid RetrieverId
    {
      get { return GetProperty(RetrieverIdProperty); }
      private set { LoadProperty(RetrieverIdProperty, value); }
    }

    public static readonly PropertyInfo<MobileDictionary<Guid, PhraseEdit>> RetrievedPhrasesProperty =
      RegisterProperty<MobileDictionary<Guid, PhraseEdit>>(c => c.RetrievedPhrases);
    public MobileDictionary<Guid, PhraseEdit> RetrievedPhrases
    {
      get { return ReadProperty(RetrievedPhrasesProperty); }
      private set { LoadProperty(RetrievedPhrasesProperty, value); }
    }

    #endregion

    #region DP_XYZ

#if !SILVERLIGHT
    public void DataPortal_Create(Criteria.ListOfPhrasesCriteria criteria)
    {
      //WE ARE GOING TO GET ALL THE PHRASES
      //WE WILL THEN ITERATE THROUGH OUR CRITERIA PHRASES, POPULATING OUR
      //RETRIEVED PHRASES 

      //INITIALIZE
      RetrieverId = Guid.NewGuid();
      if (RetrievedPhrases == null)
        RetrievedPhrases = new MobileDictionary<Guid, PhraseEdit>();

      //GET ALL PHRASES (FOR THIS USER ONLY)
      PhraseList allPhrases = PhraseList.GetAll();


      //ITERATE THROUGH CRITERIA PHRASES
      for (int i = 0; i < criteria.Phrases.Count; i++)
      {
        var criteriaPhrase = criteria.Phrases[i];

        var retrievedPhrase = (from phrase in allPhrases
                               where phrase.Text == criteriaPhrase.Text &&
                                     phrase.Language.Text == criteriaPhrase.Language.Text
                               select phrase).FirstOrDefault();

        if (!RetrievedPhrases.ContainsKey(criteriaPhrase.Id))
        {
          //if we directly add this retrievedPhrase, then it will be a child
          //we need to get the non-child version of this
          //RetrievedPhrases.Add(criteriaPhrase.Id, retrievedPhrase);
          if (retrievedPhrase != null)
          {
            var nonChildVersion = PhraseEdit.GetPhraseEdit(retrievedPhrase.Id);
            RetrievedPhrases.Add(criteriaPhrase.Id, nonChildVersion);
          }
          else
          {
            RetrievedPhrases.Add(criteriaPhrase.Id, null);
          }
        }
      }
    }
    
  
#endif

    #endregion
  }
}
