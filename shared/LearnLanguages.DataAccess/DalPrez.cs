
namespace LearnLanguages.DataAccess
{
  public static class DalPrez
  {
    public static string PhraseDalKey { get { return "PhraseDalKey"; } }
    public static string TranslationDalKey { get { return "TranslationDalKey"; } }
    public static string AllPhrasesListDalKey { get { return "AllPhrasesListDalKey"; } }

    #region ErrorMsgs
    
    public static string ErrorMsgFetchFailed(string resultMsg)
    {
      return string.Format("Fetch failed.  Result.Msg = {0}", resultMsg);
    }
    public static string ErrorMsgGeneralDataAccessException
    {
      get
      {
        return "There was a general data access exception.";
      }
    }
    public static string ErrorMsgDeleteFailedItemNotFound
    {
      get
      {
        return "Delete failed. Item Not Found";
      }
    }
    public static string ErrorMsgDeleteFailedMultipleItemsFound
    {
      get
      {
        return "Delete failed.  Multiple items found to delete.";
      }
    }

    #endregion

  }

}
