﻿using System;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.DataAccess;

#if !SILVERLIGHT
using LearnLanguages.DataAccess.Exceptions;
#endif

namespace LearnLanguages.Business
{
  [Serializable]
  public class LanguageEdit : Bases.BusinessBase<LanguageEdit, LanguageDto>, IHaveId
  {
    #region Factory Methods

    #region Silverlight Factory Methods
#if SILVERLIGHT
    /// <summary>
    /// This happens DataPortal.ProxyModes.LocalOnly
    /// </summary>
    /// <param name="callback"></param>
    public static void NewLanguageEdit(EventHandler<DataPortalResult<LanguageEdit>> callback)
    {
      DataPortal.BeginCreate<LanguageEdit>(callback, DataPortal.ProxyModes.LocalOnly);
    }
    /// <summary>
    /// This happens DataPortal.ProxyModes.LocalOnly
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callback"></param>
    public static void NewLanguageEdit(Guid id, EventHandler<DataPortalResult<LanguageEdit>> callback)
    {
      DataPortal.BeginCreate<LanguageEdit>(id, callback, DataPortal.ProxyModes.LocalOnly);
    }

    public static void GetLanguageEdit(Guid id, EventHandler<DataPortalResult<LanguageEdit>> callback)
    {
      DataPortal.BeginFetch<LanguageEdit>(id, callback);
    }
#endif
    #endregion

    #region Wpf Factory Methods
#if !SILVERLIGHT

    public static LanguageEdit NewLanguageEdit()
    {
      return DataPortal.Create<LanguageEdit>();
    }

    public static LanguageEdit NewLanguageEdit(Guid id)
    {
      return DataPortal.Create<LanguageEdit>(id);
    }

    public static LanguageEdit GetLanguageEdit(Guid id)
    {
      return DataPortal.Fetch<LanguageEdit>(id);
    }

#endif
    #endregion

    #region Shared Factory Methods

    /// <summary>
    /// Does NOT use the data portal.  This just news up an instance and plugs in the 
    /// properties bypassing property checks.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static LanguageEdit NewLanguageEdit(LanguageDto dto)
    {
      LanguageEdit retPhrase = new LanguageEdit();
      retPhrase.LoadFromDtoBypassPropertyChecks(dto);
      return retPhrase;
    }
    #endregion //Shared Factory Methods

    #endregion //Factory Methods

    #region Business Properties & Methods

    #region public string Text
    public static readonly PropertyInfo<string> TextProperty = RegisterProperty<string>(c => c.Text);
    public string Text
    {
      get { return GetProperty(TextProperty); }
      set { SetProperty(TextProperty, value); }
    }
    #endregion

    public override void LoadFromDtoBypassPropertyChecks(LanguageDto dto)
    {
      using (BypassPropertyChecks)
      {
        LoadProperty<Guid>(IdProperty, dto.Id);
        LoadProperty<string>(TextProperty, dto.Text);
      }
    }

    public override LanguageDto CreateDto()
    {
      LanguageDto retDto = new LanguageDto()
      {
        Id = this.Id,
        Text = this.Text,
      };

      return retDto;
    }

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

    #region Silverlight DP_XYZ

#if SILVERLIGHT
    /// <summary>
    /// Runs locally, does not touch server
    /// </summary>
    /// <param name="handler"></param>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public override void DataPortal_Create(LocalProxy<LanguageEdit>.CompletedHandler handler)
    {
      try
      {
        using (BypassPropertyChecks)
        {
          Id = Guid.NewGuid();
          Text = null;
        }
        handler(this, null);
      }
      catch (Exception ex)
      {
        handler(null, ex);
      }
    }

    /// <summary>
    /// Runs locally, does not touch server.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="handler"></param>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void DataPortal_Create(Guid id, LocalProxy<LanguageEdit>.CompletedHandler handler)
    {
      try
      {
        using (BypassPropertyChecks)
        {
          Id = id;
          Text = null;
        }
        handler(this, null);
      }
      catch (Exception ex)
      {
        handler(null, ex);
      }
    }

    //    //public void DataPortal_Fetch(SingleCriteria<LanguageEdit, Guid> criteria) //Guid SingleCriteria
    //    [EditorBrowsable(EditorBrowsableState.Never)]
    //    public void DataPortal_Fetch(Guid id, LocalProxy<LanguageEdit>.CompletedHandler handler) //Guid SingleCriteria
    //    {
    //      try
    //      {
    //        //Result<LanguageDto> result = LanguageDal.Fetch(id);
    //        Result<LanguageDto> result = null;
    //        throw new Exception("Need to fix all of these dp methods");
    //        if (!result.IsSuccess || result.IsError)
    //          throw new FetchFailedException(result.Msg);
    //        LanguageDto dto = result.Obj;
    //        LoadFromDtoBypassPropertyChecks(dto);
    //        handler(this, null);
    //      }
    //      catch (Exception ex)
    //      {
    //        handler(null, ex);
    //      }
    //    }

    //    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    //    public override void DataPortal_Insert(LocalProxy<LanguageEdit>.CompletedHandler handler)
    //    {
    //      try
    //      {
    //        //Dal is responsible for setting new Id
    //        LanguageDto dto = new LanguageDto()
    //        {
    //          Id = this.Id,
    //          Text = this.Text
    //        };
    //        Result<LanguageDto> result = null;
    //        //var result = LanguageDal.Insert(dto);
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
    //    public override void DataPortal_Update(LocalProxy<LanguageEdit>.CompletedHandler handler)
    //    {
    //      try
    //      {
    //        Result<LanguageDto> result = null;
    //        //Result<LanguageDto> result = LanguageDal.Update(
    //        //                                              new LanguageDto()
    //        //                                              {
    //        //                                                Id = this.Id,
    //        //                                                Text = this.Text
    //        //                                              }
    //        //                                           );
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
    //    public override void DataPortal_DeleteSelf(LocalProxy<LanguageEdit>.CompletedHandler handler)
    //    {
    //      try
    //      {
    //        Result<LanguageDto> result = null;
    //        //var result = LanguageDal.Delete(ReadProperty<Guid>(IdProperty));
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
    //    public void DataPortal_Delete(Guid criteriaId, LocalProxy<LanguageEdit>.CompletedHandler handler)
    //    {
    //      var id = criteriaId;
    //      try
    //      {
    //        Result<LanguageDto> result = null;
    //        //var result = LanguageDal.Delete(id);
    //        if (!result.IsSuccess || result.IsError)
    //          throw new DeleteFailedException(result.Msg);
    //      }
    //      catch (Exception ex)
    //      {
    //        handler(null, ex);
    //      }
    //    }

#endif
    
    #endregion

    #region WPF DP_XYZ

#if !SILVERLIGHT
    protected override void DataPortal_Create()
    {
      using (BypassPropertyChecks)
      {
        Id = Guid.NewGuid();
        Text = null;
      }
    }
    protected void DataPortal_Create(Guid id)
    {
      using (BypassPropertyChecks)
      {
        Id = id;
        Text = null;
      }
    }
    protected void DataPortal_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();
        Result<LanguageDto> result = languageDal.Fetch(id);
        if (!result.IsSuccess || result.IsError)
          throw new FetchFailedException(result.Msg);
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }
    protected override void DataPortal_Insert()
    {
      //Dal is responsible for setting new Id
      LanguageDto dto = new LanguageDto()
      {
        Id = this.Id,
        Text = this.Text
      };

      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Insert(dto);
        if (!result.IsSuccess || result.IsError)
          throw new InsertFailedException(result.Msg);

        Id = dto.Id;
      }
    }
    protected override void DataPortal_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        Result<LanguageDto> result = languageDal.Update(new LanguageDto()
                                                        {
                                                          Id = this.Id,
                                                          Text = this.Text
                                                        });
        if (!result.IsSuccess || result.IsError)
          throw new UpdateFailedException(result.Msg);
      }
    }
    protected override void DataPortal_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Delete(ReadProperty<Guid>(IdProperty));
        if (!result.IsSuccess || result.IsError)
          throw new DeleteFailedException(result.Msg);
      }
    }
    protected void DataPortal_Delete(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Delete(id);
        if (!result.IsSuccess || result.IsError)
          throw new DeleteFailedException(result.Msg);
      }
    }
#endif
    
    #endregion

    #region Child DP_XYZ
    
    public void Child_Create(Guid id)
    {
      using (BypassPropertyChecks)
      {
        this.Id = id;
        this.Text = "";
      }
    }

#if !SILVERLIGHT
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(LanguageDto dto)
    {
      //throw new Exception("Just throwing this cause I wanna see execution location. " +
      //                    "Should be 'Server'.  Here is what it actually is:       " +
      //                    Csla.ApplicationContext.LogicalExecutionLocation.ToString());

      //if (Csla.ApplicationContext.LogicalExecutionLocation != ApplicationContext.LogicalExecutionLocations.Server)
      //  throw new Exception();

      LoadFromDtoBypassPropertyChecks(dto);
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public void Child_Fetch(Guid id)
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Fetch(id);
        if (result.IsError)
          throw new FetchFailedException(result.Msg);
        LanguageDto dto = result.Obj;
        LoadFromDtoBypassPropertyChecks(dto);
      }
    }

    public void Child_Insert()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        using (BypassPropertyChecks)
        {
          var dto = CreateDto();
          var result = languageDal.Insert(dto);
          if (result.IsError)
            throw new InsertFailedException(result.Msg);
          Id = result.Obj.Id;
        }
      }
    }

    public void Child_Update()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        using (BypassPropertyChecks)
        {
          var result = languageDal.Update(CreateDto());
          if (result.IsError)
            throw new UpdateFailedException(result.Msg);
          Id = result.Obj.Id;
        }
      }
    }

    public void Child_DeleteSelf()
    {
      using (var dalManager = DalFactory.GetDalManager())
      {
        var languageDal = dalManager.GetProvider<ILanguageDal>();

        var result = languageDal.Delete(Id);
        if (result.IsError)
          throw new DeleteFailedException(result.Msg);
      }
    }
#endif

    #endregion
    #endregion
  }
}