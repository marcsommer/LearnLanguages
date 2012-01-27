using System;
using LearnLanguages.DataAccess;
using Csla;
using Csla.Serialization;
using System.Collections.Generic;
using LearnLanguages.DataAccess.Exceptions;
using System.ComponentModel;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseList : Bases.BusinessListBase<PhraseList, PhraseEdit, PhraseDto>
  {
    #region Factory Methods

    public static void GetAll(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginFetch<PhraseList>(callback);
    }

    public static void NewPhraseList(ICollection<Guid> phraseIds, EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginFetch<PhraseList>(phraseIds, callback);
    }

    public static PhraseList NewPhraseList()
    {
      return new PhraseList();
    }


    
#if SILVERLIGHT
    /// <summary>
    /// Runs locally.
    /// </summary>
    /// <param name="callback"></param>
    public static void NewPhraseList(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginCreate<PhraseList>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    /// <summary>
    /// Runs locally.
    /// </summary>
    [RunLocal]
    public static void NewPhraseList(EventHandler<DataPortalResult<PhraseList>> callback)
    {
      DataPortal.BeginCreate<PhraseList>(callback);
    }
#endif

    #endregion

    #region Data Portal methods (including child)

#if !SILVERLIGHT
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void DataPortal_Fetch(ICollection<Guid> phraseIds)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var PhraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<ICollection<PhraseDto>> result = PhraseDal.Fetch(phraseIds);
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
        var fetchedPhraseDtos = result.Obj;
        foreach (var phraseDto in fetchedPhraseDtos)
        {
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var phraseEdit = DataPortal.FetchChild<PhraseEdit>(phraseDto);
          this.Add(phraseEdit);
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
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
          //var PhraseEdit = DataPortal.CreateChild<PhraseEdit>(PhraseDto);
          var PhraseEdit = DataPortal.FetchChild<PhraseEdit>(PhraseDto);
          this.Add(PhraseEdit);
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
    public void Child_Fetch(ICollection<Guid> phraseIds)
    {
      Items.Clear();
      foreach (var id in phraseIds)
      {
        var phraseEdit = DataPortal.FetchChild<PhraseEdit>(id);
        Items.Add(phraseEdit);
      }
    }
#endif

    #endregion

    #region AddNewCore

#if SILVERLIGHT
    protected override void AddNewCore()
    {
      AddedNew += PhraseList_AddedNew; 
      base.AddNewCore();
      AddedNew -= PhraseList_AddedNew;
    }

    private void PhraseList_AddedNew(object sender, Csla.Core.AddedNewEventArgs<PhraseEdit> e)
    {
      //CustomIdentity.CheckAuthentication();
      var phraseEdit = e.NewObject;
      phraseEdit.LoadCurrentUser(); 
      //var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      //phraseEdit.UserId = identity.UserId;
      //phraseEdit.Username = identity.Name;
    }
#else
    protected override PhraseEdit AddNewCore()
    {
      //SERVER
      var phraseEdit = base.AddNewCore();
      phraseEdit.LoadCurrentUser();
      return phraseEdit;
    }
#endif

    #endregion
  }
}
