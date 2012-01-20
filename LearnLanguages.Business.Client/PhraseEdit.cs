using System;
using Csla;
using System.ComponentModel;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
#if !SILVERLIGHT
using LearnLanguages.DataAccess.Exceptions;
#endif
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;


namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseEdit : LearnLanguages.Business.Bases.BusinessBase<PhraseEdit, PhraseDto>, IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new PhraseEdit sync
    /// </summary>
    public static PhraseEdit NewPhraseEdit()
    {
      return DataPortal.Create<PhraseEdit>();
    }
    /// <summary>
    /// Creates a new PhraseEdit sync with given id.
    /// </summary>
    public static PhraseEdit NewPhraseEdit(Guid id)
    {
      return DataPortal.Create<PhraseEdit>(id);
    }
    /// <summary>
    /// Fetches a PhraseEdit sync with given id
    /// </summary>
    public static PhraseEdit GetPhraseEdit(Guid id)
    {
      return DataPortal.Fetch<PhraseEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    /// <summary>
    /// This happens DataPortal.ProxyModes.LocalOnly
    /// </summary>
    /// <param name="callback"></param>
    public static void NewPhraseEdit(EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      //DataPortal.BeginCreate<PhraseEdit>(callback, DataPortal.ProxyModes.LocalOnly);
      DataPortal.BeginCreate<PhraseEdit>(callback);
    }

    ///// <summary>
    ///// This happens DataPortal.ProxyModes.LocalOnly
    ///// </summary>
    ///// <param name="id"></param>
    ///// <param name="callback"></param>
    //public static void NewPhraseEdit(Guid id, EventHandler<DataPortalResult<PhraseEdit>> callback)
    //{
    //  DataPortal.BeginCreate<PhraseEdit>(id, callback, DataPortal.ProxyModes.LocalOnly);
    //  //DataPortal.BeginCreate<PhraseEdit>(id, callback);
    //}

    public static void GetPhraseEdit(Guid id, EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      DataPortal.BeginFetch<PhraseEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods
    //PHRASE
    #region public string Text
    public static readonly PropertyInfo<string> TextProperty = RegisterProperty<string>(c => c.Text);
    public string Text
    {
      get { return GetProperty(TextProperty); }
      set { SetProperty(TextProperty, value); }
    }
    #endregion

    //LANGUAGE
    #region public Guid LanguageId
    public static readonly PropertyInfo<Guid> LanguageIdProperty = RegisterProperty<Guid>(c => c.LanguageId);
    //[Display(ResourceType = typeof(ModelResources), Name = "Phrase_Language_DisplayName")]
    //[Required(ErrorMessageResourceType=typeof(ModelResources), ErrorMessageResourceName="Phrase_Language_RequiredErrorMessage")]
    public Guid LanguageId
    {
      get { return GetProperty(LanguageIdProperty); }
      set { SetProperty(LanguageIdProperty, value); }
    }
    #endregion
    #region public LanguageEdit Language
    public static readonly PropertyInfo<LanguageEdit> LanguageProperty =
      RegisterProperty<LanguageEdit>(c => c.Language, RelationshipTypes.Child);
    public LanguageEdit Language
    {
      get { return GetProperty(LanguageProperty); }
      private set { LoadProperty(LanguageProperty, value); }
    }
    #endregion
    
    //USER
    #region public Guid UserId
    public static readonly PropertyInfo<Guid> UserIdProperty = RegisterProperty<Guid>(c => c.UserId);
    public Guid UserId
    {
      get { return GetProperty(UserIdProperty); }
      set { SetProperty(UserIdProperty, value); }
    }
    #endregion
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

    public override void LoadFromDtoBypassPropertyChecksImpl(PhraseDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(TextProperty, dto.Text);
        LoadProperty<Guid>(LanguageIdProperty, dto.LanguageId);
        if (dto.LanguageId != Guid.Empty)
          Language = DataPortal.FetchChild<LanguageEdit>(dto.LanguageId);
        LoadProperty<Guid>(UserIdProperty, dto.UserId);
        LoadProperty<string>(UsernameProperty, dto.Username);
        if (!string.IsNullOrEmpty(dto.Username))
          User = DataPortal.FetchChild<CustomIdentity>(dto.Username);
      }
    }
    public override PhraseDto CreateDto()
    {
      PhraseDto retDto = new PhraseDto(){
                                          Id = this.Id,
                                          Text = this.Text,
                                          LanguageId = this.LanguageId,
                                          UserId = this.UserId,
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
        //LanguageId = LanguageEdit.GetLanguageEdit
        Text = DalResources.DefaultNewPhraseText;
        Language = DataPortal.FetchChild<LanguageEdit>(LanguageId);
        UserId = Guid.Empty;
        Username = DalResources.DefaultNewPhraseUsername;
      }
      BusinessRules.CheckRules();
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      // TODO: add validation rules
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(LanguageIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(TextProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UserIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(TextProperty, 1));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: PhraseEdit Authorization: add object-level authorization rules
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
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.New(null);
        if (!result.IsSuccess || result.IsError)
          throw new CreateFailedException(result.Msg);
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        Result<PhraseDto> result = phraseDal.Fetch(id);
        if (!result.IsSuccess || result.IsError)
          throw new FetchFailedException(result.Msg);
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      //PhraseDto dto = new PhraseDto()
      //{
      //  Id = this.Id,
      //  LanguageId = this.LanguageId,
      //  Text = this.Text
      //};
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var dto = CreateDto();
        var result = phraseDal.Insert(dto);
        if (!result.IsSuccess || result.IsError)
          throw new InsertFailedException(result.Msg);
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
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var dto = CreateDto();
        Result<PhraseDto> result = phraseDal.Update(dto);
        if (!result.IsSuccess || result.IsError)
          throw new UpdateFailedException(result.Msg);
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
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess || result.IsError)
          throw new DeleteFailedException(result.Msg);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Delete(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Delete(id);
        if (!result.IsSuccess || result.IsError)
          throw new DeleteFailedException(result.Msg);
      }
    }

#endif
    #endregion //Wpf DP_XYZ
    
    #region Child DP_XYZ
    
#if !SILVERLIGHT

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        PhraseDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    public void Child_Insert()
    {   
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = phraseDal.Insert(dto);
          if (result.IsError)
            throw new InsertFailedException(result.Msg);
          LoadFromDtoBypassPropertyChecks(result.Obj);
        }
      }
    }

    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        var dto = CreateDto();
        var result = phraseDal.Update(dto);
        if (result.IsError)
          throw new UpdateFailedException(result.Msg);
        LoadFromDtoBypassPropertyChecks(result.Obj);
      }
    }

    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        var result = phraseDal.Delete(Id);
        if (result.IsError)
          throw new DeleteFailedException(result.Msg); 
      }
    }

#endif

    #endregion

    #endregion
  }
}
