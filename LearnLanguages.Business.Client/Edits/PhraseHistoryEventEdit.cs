using System;
using Csla;
using System.ComponentModel;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;


namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseHistoryEdit : Common.CslaBases.BusinessBase<PhraseHistoryEdit, PhraseHistoryEventDto>,
                               IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new PhraseHistoryEdit sync
    /// </summary>
    public static PhraseHistoryEdit NewPhraseHistoryEdit()
    {
      return DataPortal.Create<PhraseHistoryEdit>();
    }
    /// <summary>
    /// Fetches a PhraseHistoryEdit sync with given id
    /// </summary>
    public static PhraseHistoryEdit GetPhraseHistoryEdit(Guid id)
    {
      return DataPortal.Fetch<PhraseHistoryEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static void NewPhraseHistoryEdit(EventHandler<DataPortalResult<PhraseHistoryEdit>> callback)
    {
      //DataPortal.BeginCreate<PhraseHistoryEdit>(callback, DataPortal.ProxyModes.LocalOnly);
      DataPortal.BeginCreate<PhraseHistoryEdit>(callback);
    }

    public static void GetPhraseHistoryEdit(Guid id, EventHandler<DataPortalResult<PhraseHistoryEdit>> callback)
    {
      DataPortal.BeginFetch<PhraseHistoryEdit>(id, callback);
    }

    //public static void GetPhraseHistoryEdit(EventHandler<DataPortalResult<PhraseHistoryEdit>> callback)
    //{
    //  DataPortal.BeginFetch<PhraseHistoryEdit>(callback);
    //}
#endif
    #endregion
    #endregion

    #region Business Properties & Methods
    //NATIVE LANGUAGE TEXT
    #region public string NativeLanguageText
    public static readonly PropertyInfo<string> NativeLanguageTextProperty = RegisterProperty<string>(c => c.NativeLanguageText);
    public string NativeLanguageText
    {
      get { return GetProperty(NativeLanguageTextProperty); }
      set { SetProperty(NativeLanguageTextProperty, value); }
    }
    #endregion
    
    //USER
    #region public string Username
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    public string Username
    {
      get { return GetProperty(UsernameProperty); }
      set { SetProperty(UsernameProperty, value); }
    }
    #endregion
    #region public CustomIdentity User
    public static readonly PropertyInfo<CustomIdentity> UserProperty =
      RegisterProperty<CustomIdentity>(c => c.User, RelationshipTypes.Child);
    public CustomIdentity User
    {
      get { return GetProperty(UserProperty); }
      private set { LoadProperty(UserProperty, value); }
    }
    #endregion

    public override void LoadFromDtoBypassPropertyChecksImpl(PhraseHistoryDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(NativeLanguageTextProperty, dto.NativeLanguageText);
        LoadProperty<string>(UsernameProperty, dto.Username);
        if (!string.IsNullOrEmpty(dto.Username))
          User = DataPortal.FetchChild<CustomIdentity>(dto.Username);
      }
    }
    public override PhraseHistoryDto CreateDto()
    {
      PhraseHistoryDto retDto = new PhraseHistoryDto(){
                                          Id = this.Id,
                                          NativeLanguageText = this.NativeLanguageText,
                                          Username = this.Username
                                        };
      return retDto;
    }

    /// <summary>
    /// Begins to persist object
    /// </summary>
    public override void BeginSave(bool forceUpdate, EventHandler<Csla.Core.SavedEventArgs> handler, object userState)
    {
      base.BeginSave(forceUpdate, handler, userState);
    }

    /// <summary>
    /// Loads the default properties, including generating a new Id, inside of a using (BypassPropertyChecks) block.
    /// </summary>
    private void LoadDefaults()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.NewGuid();
        NativeLanguageText = BusinessResources.DefaultNewPhraseHistoryNativeLanguageText;
        LoadCurrentUser();
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    /// <summary>
    /// Does CheckAuthentication().  Then Loads the PhraseHistory with the current user, userid, username.
    /// </summary>
    internal void LoadCurrentUser()
    {
      CustomIdentity.CheckAuthentication();
      var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      Username = identity.Name;
      User = identity;
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      // TODO: add validation rules
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NativeLanguageTextProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(NativeLanguageTextProperty, 
                                                                 int.Parse(BusinessResources.MinNativeLanguageTextLength)));

      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(UsernameProperty,
                                                                 int.Parse(BusinessResources.MinUsernameLength)));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: PhraseHistoryEdit Authorization: add object-level authorization rules
      // Csla.Rules.CommonRules.Required
    }

    #endregion

    #region Data Access (This is run on the server, unless run local set)

    #region Wpf DP_XYZ
#if !SILVERLIGHT

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
        var result = studyDataDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        PhraseHistoryDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    //[Transactional(TransactionalTypes.TransactionScope)]
    //protected void DataPortal_Fetch(Guid id)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
    //    Result<PhraseHistoryDto> result = studyDataDal.Fetch(id);
    //    if (!result.IsSuccess)
    //    {
    //      Exception error = result.GetExceptionFromInfo();
    //      if (error != null)
    //        throw error;
    //      else
    //        throw new FetchFailedException(result.Msg);
    //    }
    //    PhraseHistoryDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}
    //[Transactional(TransactionalTypes.TransactionScope)]
    //protected void DataPortal_Fetch()
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
    //    Result<bool> resultExists = studyDataDal.PhraseHistoryExistsForCurrentUser();
    //    if (!resultExists.IsSuccess)
    //    {
    //      Exception error = resultExists.GetExceptionFromInfo();
    //      if (error != null)
    //        throw error;
    //      else
    //        throw new FetchFailedException(resultExists.Msg);
    //    }
    //    var userHasPhraseHistory = resultExists.Obj;

    //    //POPULATE OUR STUDY DATA DTO
    //    PhraseHistoryDto dto = null;

    //    if (userHasPhraseHistory)
    //    {
    //      //THE USER HAS STUDY DATA, SO FETCH IT
    //      Result<PhraseHistoryDto> resultFetch = studyDataDal.FetchForCurrentUser();
    //      if (!resultFetch.IsSuccess)
    //      {
    //        Exception error = resultFetch.GetExceptionFromInfo();
    //        if (error != null)
    //          throw error;
    //        else
    //          throw new FetchFailedException(resultFetch.Msg);
    //      }

    //      dto = resultFetch.Obj;
    //    }
    //    else
    //    {
    //      //THE USER DOESN'T HAVE STUDY DATA
    //      //SET PROPS ACCORDINGLY
    //      dto = new PhraseHistoryDto()
    //      {
    //        Id = Guid.Empty,
    //        NativeLanguageText = "",
    //        Username = Csla.ApplicationContext.User.Identity.Name
    //      };
    //    }

    //    //OUR DTO IS NOW POPULATED
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      //PhraseHistoryDto dto = new PhraseHistoryDto()
      //{
      //  Id = this.Id,
      //  LanguageId = this.LanguageId,
      //  Text = this.Text
      //};
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
        var dto = CreateDto();
        var result = studyDataDal.Insert(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new InsertFailedException(result.Msg);
        }
        //SetIdBypassPropertyChecks(result.Obj.Id);
        //Loading the whole Dto now because I think the insert may affect LanguageId and UserId, and the object
        //may need to load new LanguageEdit child, new languageId, etc.
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
        var dto = CreateDto();
        Result<PhraseHistoryDto> result = studyDataDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException(result.Msg);
        }
        //SetIdBypassPropertyChecks(result.Obj.Id);
        //Loading the whole Dto now because I think the insert may affect LanguageId and UserId, and the object
        //may need to load new LanguageEdit child, new languageId, etc.
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
        var result = studyDataDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Delete(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
        var result = studyDataDal.Delete(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

#endif
    #endregion //Wpf DP_XYZ
    
    #region Child DP_XYZ
    
#if !SILVERLIGHT
    
    //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //public void Child_Fetch(Guid id)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var PhraseHistoryDal = dalManager.GetProvider<IPhraseHistoryDal>();
    //    var result = PhraseHistoryDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    PhraseHistoryDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(PhraseHistoryDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var PhraseHistoryDal = dalManager.GetProvider<IPhraseHistoryDal>();
      //  var result = PhraseHistoryDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  PhraseHistoryDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //public void Child_Fetch(Guid id)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
    //    var result = studyDataDal.Fetch(id);
    //    if (result.IsError)
    //      throw new FetchFailedException(result.Msg);
    //    PhraseHistoryDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {   
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = studyDataDal.Insert(dto);
          if (!result.IsSuccess)
          {
            Exception error = result.GetExceptionFromInfo();
            if (error != null)
              throw error;
            else
              throw new InsertFailedException(result.Msg);
          }
          LoadFromDtoBypassPropertyChecks(result.Obj);
        }
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();

        var dto = CreateDto();
        var result = studyDataDal.Update(dto);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new UpdateFailedException(result.Msg);
        }
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var studyDataDal = dalManager.GetProvider<IPhraseHistoryDal>();

        var result = studyDataDal.Delete(Id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new DeleteFailedException(result.Msg);
        }
      }
    }

#endif

    #endregion

    #endregion
  }
}
