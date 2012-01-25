using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;

namespace LearnLanguages.Business
{
  [Serializable]
  public class TranslationList : Bases.BusinessListBase<TranslationList, TranslationEdit, TranslationDto>
  {
    public static void GetAll(EventHandler<DataPortalResult<TranslationList>> callback)
    {
      DataPortal.BeginFetch<TranslationList>(callback);
    }

    public static void NewTranslationList(ICollection<Guid> translationIds, EventHandler<DataPortalResult<TranslationList>> callback)
    {
      DataPortal.BeginFetch<TranslationList>(translationIds, callback);
    }

#if !SILVERLIGHT
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(ICollection<Guid> translationIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var TranslationDal = dalManager.GetProvider<ITranslationDal>();

        Result<ICollection<TranslationDto>> result = TranslationDal.Fetch(translationIds);
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
        var fetchedTranslationDtos = result.Obj;
        foreach (var translationDto in fetchedTranslationDtos)
        {
          //var TranslationEdit = DataPortal.CreateChild<TranslationEdit>(TranslationDto);
          var translationEdit = DataPortal.FetchChild<TranslationEdit>(translationDto);
          this.Add(translationEdit);
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var TranslationDal = dalManager.GetProvider<ITranslationDal>();

        Result<ICollection<TranslationDto>> result = TranslationDal.GetAll();
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
        var allTranslationDtos = result.Obj;
        foreach (var TranslationDto in allTranslationDtos)
        {
          //var TranslationEdit = DataPortal.CreateChild<TranslationEdit>(TranslationDto);
          var TranslationEdit = DataPortal.FetchChild<TranslationEdit>(TranslationDto);
          this.Add(TranslationEdit);
        }
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        base.Child_Update();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Child_Fetch(ICollection<Guid> translationIds)
    {
      try
      {
        throw new Exception("I just wanted to see where this gets called");
      }
      catch
      {
        //
      }
      Items.Clear();
      foreach (var id in translationIds)
      {
        var translationEdit = DataPortal.FetchChild<TranslationEdit>(id);
        Items.Add(translationEdit);
      }
    }
#endif
  }
}
