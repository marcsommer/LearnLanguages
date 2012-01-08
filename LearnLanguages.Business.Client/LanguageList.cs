using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Business
{
  [Serializable]
  public class LanguageList : Bases.BusinessListBase<LanguageList, LanguageEdit, LanguageDto>
  {
    public static void GetAll(EventHandler<DataPortalResult<LanguageList>> callback)
    {
      DataPortal.BeginFetch<LanguageList>(callback);
    }

#if !SILVERLIGHT
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        Result<ICollection<LanguageDto>> result = languageDal.GetAll();
        if (!result.IsSuccess || result.IsError)
        {
          if (result.Info != null)
          {
            var ex = result.GetExceptionFromInfo();
            if (ex != null)
              throw new FetchFailedException(ex.Message);
            else
              throw new FetchFailedException();
          }
          else
            throw new FetchFailedException();
        }

        //RESULT WAS SUCCESSFUL
        var allLanguageDtos = result.Obj;
        foreach (var languageDto in allLanguageDtos)
        {
          //does not use dataportal
          var languageEdit = DataPortal.CreateChild<LanguageEdit>(languageDto);
          Add(languageEdit);
        }
      }
    }
#endif

  }
}
