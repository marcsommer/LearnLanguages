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
      DataPortal.BeginCreate<PhraseEdit>(callback, DataPortal.ProxyModes.LocalOnly);
    }
    /// <summary>
    /// This happens DataPortal.ProxyModes.LocalOnly
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callback"></param>
    public static void NewPhraseEdit(Guid id, EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      DataPortal.BeginCreate<PhraseEdit>(id, callback, DataPortal.ProxyModes.LocalOnly);
    }

    public static void GetPhraseEdit(Guid id, EventHandler<DataPortalResult<PhraseEdit>> callback)
    {
      DataPortal.BeginFetch<PhraseEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods

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
    
    #region public string Text
    public static readonly PropertyInfo<string> TextProperty = RegisterProperty<string>(c => c.Text);
    public string Text
    {
      get { return GetProperty(TextProperty); }
      set { SetProperty(TextProperty, value); }
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

    public override void LoadFromDtoBypassPropertyChecks(PhraseDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(TextProperty, dto.Text);
        LoadProperty<Guid>(LanguageIdProperty, dto.LanguageId);
        if (dto.LanguageId != Guid.Empty)
          Language = DataPortal.FetchChild<LanguageEdit>(dto.LanguageId);
      }
    }

    public override PhraseDto CreateDto()
    {
      PhraseDto retDto = new PhraseDto(){
                                          Id = this.Id,
                                          Text = this.Text,
                                          LanguageId = this.LanguageId
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
    
    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();

      // TODO: add validation rules
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(LanguageIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(TextProperty));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: add object-level authorization rules
      // Csla.Rules.CommonRules.Required
    }

    #endregion

    #region Data Access (This is run on the server, unless run local set)
 
//    #region Silverlight DP_XYZ
//#if SILVERLIGHT
//    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//    public override void DataPortal_Create(LocalProxy<PhraseEdit>.CompletedHandler handler)
//    {
//      try
//      {
//        using (BypassPropertyChecks)
//        {
//          Id = Guid.NewGuid();
//          LanguageId = Guid.Empty;
//          Text = null;
//          //Language = null;
//          Language = DataPortal.CreateChild<LanguageEdit>();
//        }
//        handler(this, null);
//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }

//    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//    public void DataPortal_Create(Guid id, LocalProxy<PhraseEdit>.CompletedHandler handler)
//    {
//      try
//      {
//        using (BypassPropertyChecks)
//        {
//          Id = id;
//          LanguageId = Guid.Empty;
//          Text = null;
//          //Language = null;
//          Language = DataPortal.CreateChild<LanguageEdit>();
//        }
//        handler(this, null);
//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }

//    //public void DataPortal_Fetch(SingleCriteria<PhraseEdit, Guid> criteria) //Guid SingleCriteria
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public void DataPortal_Fetch(Guid criteria, LocalProxy<PhraseEdit>.CompletedHandler handler) //Guid SingleCriteria
//    {
//      Guid id = criteria;

//      try
//      {
//        Result<PhraseDto> result = PhraseDal.Fetch(id);
//        if (!result.IsSuccess || result.IsError)
//          throw new FetchFailedException(result.Msg);
//        PhraseDto dto = result.Obj;
//        LoadFromDtoBypassPropertyChecks(dto);
//        handler(this, null);
//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }

//    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//    public override void DataPortal_Insert(LocalProxy<PhraseEdit>.CompletedHandler handler)
//    {
//      try
//      {
//        //Dal is responsible for setting new Id
//        //PhraseDto dto = new PhraseDto()
//        //{
//        //  Id = this.Id,
//        //  LanguageId = this.LanguageId,
//        //  Text = this.Text
//        //};
//        var dto = CreateDto();
//        var result = PhraseDal.Insert(dto);
//        if (!result.IsSuccess || result.IsError)
//          throw new InsertFailedException(result.Msg);

//        Id = dto.Id;
//        handler(this, null);
//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }

//    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//    public override void DataPortal_Update(LocalProxy<PhraseEdit>.CompletedHandler handler)
//    {
//      try
//      {
//        var dto = CreateDto();
//        Result<PhraseDto> result = PhraseDal.Update(dto);
//        if (!result.IsSuccess || result.IsError)
//          throw new UpdateFailedException(result.Msg);

//        handler(this, null);
//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }

//    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//    public override void DataPortal_DeleteSelf(LocalProxy<PhraseEdit>.CompletedHandler handler)
//    {
//      try
//      {
//        var result = PhraseDal.Delete(ReadProperty<Guid>(IdProperty));
//        if (!result.IsSuccess || result.IsError)
//          throw new DeleteFailedException(result.Msg);

//        handler(this, null);
//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }

//    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//    public void DataPortal_Delete(Guid criteriaId, LocalProxy<PhraseEdit>.CompletedHandler handler)
//    {
//      var id = criteriaId;
//      try
//      {
//        var result = PhraseDal.Delete(id);
//        if (!result.IsSuccess || result.IsError)
//          throw new DeleteFailedException(result.Msg);

//      }
//      catch (Exception ex)
//      {
//        handler(null, ex);
//      }
//    }
//#endif
//    #endregion //Silverlight DP_XYZ

    #region Wpf DP_XYZ
#if !SILVERLIGHT

    protected override void DataPortal_Create()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.NewGuid();
        LanguageId = Guid.Empty;
        Text = null;
        Language = null;
      }
    }
    protected void DataPortal_Create(Guid id)
    {
      using (BypassPropertyChecks)
      {
        Id = id;
        LanguageId = Guid.Empty;
        Text = null;
        Language = null;
      }
    }
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
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      PhraseDto dto = new PhraseDto()
      {
        Id = this.Id,
        LanguageId = this.LanguageId,
        Text = this.Text
      };
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();
        var result = phraseDal.Insert(dto);
        if (!result.IsSuccess || result.IsError)
          throw new InsertFailedException(result.Msg);
      }
    }
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        Result<PhraseDto> result = phraseDal.Update(
                                                      new PhraseDto()
                                                      {
                                                        Id = this.Id,
                                                        LanguageId = this.LanguageId,
                                                        Text = this.Text
                                                      }
                                                   );
        if (!result.IsSuccess || result.IsError)
          throw new UpdateFailedException(result.Msg);
      }
    }
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

    public void Child_Create(Guid id)
    {
      using (BypassPropertyChecks)
      {
        this.Id = id;
        this.Text = "";
        this.LanguageId = Guid.Empty;
        this.Language = null;
      }
    }

#if SILVERLIGHT
    /// <summary>
    /// Just throws exception.  I assume this will not be run.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      //TODO: TAKE OUT CHILD FETCH IN SILVERLIGHT BLOCK WHEN I KNOW THAT CHILD FETCH ALWAYS RUNS ON SERVER
      throw new Exception("Child_Fetch has been executed on silverlight client.  I assumed this wouldn't happen.");
      //var result = PhraseDal.Fetch(id);
      //if (result.IsError)
      //  throw new FetchFailedException(result.Msg);
      //PhraseDto dto = result.Obj;
      //LoadFromDtoBypassPropertyChecks(dto);
    }
#endif
    
#if !SILVERLIGHT
     
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(PhraseDto dto)
    {
      //Debug
      //throw new Exception("Just throwing this cause I wanna see execution location. " +
      //                    "Should be 'Server'.  Here is what it actually is:       " +
      //                    Csla.ApplicationContext.LogicalExecutionLocation.ToString());

      //if (Csla.ApplicationContext.LogicalExecutionLocation != ApplicationContext.LogicalExecutionLocations.Server)
      //  throw new Exception();
      throw new Exception("debug: why am i calling this method: childfetch(dto)?");
      LoadFromDtoBypassPropertyChecks(dto);
    }

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
            throw new InsertFailedException(result.Msg); //hack: string literal exception message
          Id = result.Obj.Id;
        }
      }
    }

    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = phraseDal.Update(dto);
          if (result.IsError)
            throw new UpdateFailedException(result.Msg); //hack: string literal exception message
          Id = result.Obj.Id;
        }
      }
    }

    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var phraseDal = dalManager.GetProvider<IPhraseDal>();

        var result = phraseDal.Delete(Id);
        if (result.IsError)
          throw new DeleteFailedException(result.Msg); //hack: string literal exception message
      }
    }

#endif

    #endregion

    #endregion
  }
}
