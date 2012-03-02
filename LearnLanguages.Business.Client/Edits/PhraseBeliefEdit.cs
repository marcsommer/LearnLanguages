﻿using System;
using Csla;
using System.ComponentModel;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess.Exceptions;
using LearnLanguages.DataAccess;
using LearnLanguages.Business.Security;
using LearnLanguages.Common.CslaBases;


namespace LearnLanguages.Business
{
  [Serializable]
  public class PhraseBeliefEdit : BusinessBase<PhraseBeliefEdit, PhraseBeliefDto>, 
                                  IHaveId
  {
    #region Factory Methods
    #region Wpf Factory Methods
#if !SILVERLIGHT

    /// <summary>
    /// Creates a new PhraseBeliefEdit sync
    /// </summary>
    public static PhraseBeliefEdit NewPhraseBeliefEdit()
    {
      return DataPortal.Create<PhraseBeliefEdit>();
    }
    
    /// <summary>
    /// Fetches a PhraseBeliefEdit sync with given id
    /// </summary>
    public static PhraseBeliefEdit GetPhraseBeliefEdit(Guid id)
    {
      return DataPortal.Fetch<PhraseBeliefEdit>(id);
    }

#endif
    #endregion

    #region Silverlight Factory Methods
#if SILVERLIGHT

    public static void NewPhraseBeliefEdit(EventHandler<DataPortalResult<PhraseBeliefEdit>> callback)
    {
      //DataPortal.BeginCreate<PhraseBeliefEdit>(callback, DataPortal.ProxyModes.LocalOnly);
      DataPortal.BeginCreate<PhraseBeliefEdit>(callback);
    }

    public static void GetPhraseBeliefEdit(Guid id, EventHandler<DataPortalResult<PhraseBeliefEdit>> callback)
    {
      DataPortal.BeginFetch<PhraseBeliefEdit>(id, callback);
    }

#endif
    #endregion
    #endregion

    #region Business Properties & Methods

    //SCALAR
    #region public DateTime Date
    public static readonly PropertyInfo<DateTime> DateProperty = RegisterProperty<DateTime>(c => c.Date);
    public DateTime Date
    {
      get { return GetProperty(DateProperty); }
      set { SetProperty(DateProperty, value); }
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
    #region public double Strength
    public static readonly PropertyInfo<double> StrengthProperty = RegisterProperty<double>(c => c.Strength);
    public double Strength
    {
      get { return GetProperty(StrengthProperty); }
      set { SetProperty(StrengthProperty, value); }
    }
    #endregion

    //PHRASE
    #region public Guid PhraseId
    public static readonly PropertyInfo<Guid> PhraseIdProperty = RegisterProperty<Guid>(c => c.PhraseId);
    //[Display(ResourceType = typeof(ModelResources), Name = "DefaultStudyAdvisorKnowledgeBelief_Phrase_DisplayName")]
    //[Required(ErrorMessageResourceType=typeof(ModelResources), ErrorMessageResourceName="DefaultStudyAdvisorKnowledgeBelief_Phrase_RequiredErrorMessage")]
    public Guid PhraseId
    {
      get { return GetProperty(PhraseIdProperty); }
      set { SetProperty(PhraseIdProperty, value); }
    }
    #endregion
    #region public PhraseEdit Phrase
    public static readonly PropertyInfo<PhraseEdit> PhraseProperty =
      RegisterProperty<PhraseEdit>(c => c.Phrase, RelationshipTypes.Child);
    public PhraseEdit Phrase
    {
      get { return GetProperty(PhraseProperty); }
      set 
      {
        LoadProperty(PhraseProperty, value);

        if (value != null)
          PhraseId = value.Id;
        else
          PhraseId = Guid.Empty;
      }
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

    public override void LoadFromDtoBypassPropertyChecksImpl(PhraseBeliefDto dto)
    {
      using (BypassPropertyChecks)
      {
        //ID
        LoadProperty<Guid>(IdProperty, dto.Id);

        //SCALAR
        LoadProperty<string>(TextProperty, dto.Text);
        LoadProperty<DateTime>(DateProperty, dto.Date);
        LoadProperty<double>(StrengthProperty, dto.Strength);

        //PHRASE
        LoadProperty<Guid>(PhraseIdProperty, dto.PhraseId);
        if (dto.PhraseId != Guid.Empty)
          Phrase = DataPortal.FetchChild<PhraseEdit>(dto.PhraseId);

        //USER
        LoadProperty<Guid>(UserIdProperty, dto.UserId);
        LoadProperty<string>(UsernameProperty, dto.Username);
        if (!string.IsNullOrEmpty(dto.Username))
          User = DataPortal.FetchChild<CustomIdentity>(dto.Username);
      }
    }
    public override PhraseBeliefDto CreateDto()
    {
      PhraseBeliefDto retDto = new PhraseBeliefDto(){
                                          Id = this.Id,
                                          Date = this.Date,
                                          Text = this.Text,
                                          Strength = this.Strength,
                                          PhraseId = this.PhraseId,
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
      var currentUsername = Csla.ApplicationContext.User.Identity.Name;
      using (BypassPropertyChecks)
      {
        Id = Guid.NewGuid();
        //LanguageId = LanguageEdit.GetLanguageEdit
        Date = DateTime.Now + TimeSpan.FromDays(1.0d);//defaults to tomorrow
        Text = BusinessResources.DefaultPhraseBeliefText;
        Strength = double.Parse(BusinessResources.DefaultPhraseBeliefStrength);
        PhraseId = Guid.Empty;
        UserId = Guid.Empty;
        Username = "";
      }
      BusinessRules.CheckRules();
    }

    public int GetEditLevel()
    {
      return EditLevel;
    }

    /// <summary>
    /// Does CheckAuthentication().  Then Loads the line with the current user, userid, username.
    /// </summary>
    internal void LoadCurrentUser()
    {
      CustomIdentity.CheckAuthentication();
      var identity = (CustomIdentity)Csla.ApplicationContext.User.Identity;
      UserId = identity.UserId;
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(PhraseIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UserIdProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(UsernameProperty));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MinValue<double>(StrengthProperty, 0));
    }

    #endregion

    #region Authorization Rules

    public static void AddObjectAuthorizationRules()
    {
      // TODO: PhraseBeliefEdit Authorization: add object-level authorization rules
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
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var result = beliefDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        PhraseBeliefDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);

        //PHRASE CHILD
        Phrase = DataPortal.CreateChild<PhraseEdit>();
      }
    }
    //[Transactional(TransactionalTypes.TransactionScope)]
    //protected void DataPortal_Create(string languageText)
    //{
    //  using (var dalManager = DalFactory.GetDalManager())
    //  {
    //    //LanguageEdit languageEdit = LanguageEdit.GetLanguageEdit(languageText);
    //    //LanguageEdit languageEdit = DataPortal.FetchChild<LanguageEdit>(languageText);

    //    var beliefDal = dalManager.GetProvider<IDefaultStudyAdvisorKnowledgeBeliefDal>();
    //    var result = beliefDal.New(languageText);
    //    if (!result.IsSuccess)
    //    {
    //      Exception error = result.GetExceptionFromInfo();
    //      if (error != null)
    //        throw error;
    //      else
    //        throw new CreateFailedException(result.Msg);
    //    }
    //    PhraseBeliefDto dto = result.Obj;
    //    LoadFromDtoBypassPropertyChecks(dto);
    //  }
    //}

    [Transactional(TransactionalTypes.TransactionScope)]
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        Result<PhraseBeliefDto> result = beliefDal.Fetch(id);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new FetchFailedException(result.Msg);
        }
        PhraseBeliefDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      //PhraseBeliefDto dto = new PhraseBeliefDto()
      //{
      //  Id = this.Id,
      //  LanguageId = this.LanguageId,
      //  Text = this.Text
      //};
      using (var dalManager = DalFactory.GetDalManager())
      {
        //NEED TO UPDATE CHILDREN FIRST
        FieldManager.UpdateChildren(this);

        //UPDATE CHILDREN WILL INSERT/UPDATE PHRASE AS NEEDED AND THUS CHANGE PHRASE ID
        PhraseId = Phrase.Id;

        //UPDATE OUR BELIEF ITSELF
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var dto = CreateDto();
        var result = beliefDal.Insert(dto);
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
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var dto = CreateDto();
        Result<PhraseBeliefDto> result = beliefDal.Update(dto);
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
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var result = beliefDal.Delete(ReadProperty<Guid>(IdProperty));
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
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var result = beliefDal.Delete(id);
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
    
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Child_Create()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var result = beliefDal.New(null);
        if (!result.IsSuccess)
        {
          Exception error = result.GetExceptionFromInfo();
          if (error != null)
            throw error;
          else
            throw new CreateFailedException(result.Msg);
        }
        PhraseBeliefDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(PhraseBeliefDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);

      //using (var dalManager = DalFactory.GetDalManager())
      //{
      //  var beliefDal = dalManager.GetProvider<IDefaultStudyAdvisorKnowledgeBeliefDal>();
      //  var result = beliefDal.Fetch(id);
      //  if (result.IsError)
      //    throw new FetchFailedException(result.Msg);
      //  PhraseBeliefDto dto = result.Obj;
      //  LoadFromDtoBypassPropertyChecks(dto);
      //}
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        var result = beliefDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        PhraseBeliefDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Insert()
    {   
      using (var dalManager = DalFactory.GetDalManager())
      {
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();
        using (BypassPropertyChecks)
        {
          FieldManager.UpdateChildren(this);
          PhraseId = Phrase.Id;

          var dto = CreateDto();
          var result = beliefDal.Insert(dto);
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
        FieldManager.UpdateChildren(this);
        PhraseId = Phrase.Id;

        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        var dto = CreateDto();
        var result = beliefDal.Update(dto);
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
        var beliefDal = dalManager.GetProvider<IPhraseBeliefDal>();

        var result = beliefDal.Delete(Id);
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
