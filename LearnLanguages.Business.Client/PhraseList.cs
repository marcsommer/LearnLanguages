using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseList : Bases.BusinessListBase<PhraseList, PhraseEdit, PhraseDto>
  {
    public static void GetAll(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginFetch<PhraseList>(callback);
    }

#if !SILVERLIGHT
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var PhraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = PhraseDal.GetAll();
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
        var allPhraseDtos = result.Obj;
        foreach (var PhraseDto in allPhraseDtos)
        {
          var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          Add(PhraseEdit);
        }
      }
    }
#endif

  }
}
