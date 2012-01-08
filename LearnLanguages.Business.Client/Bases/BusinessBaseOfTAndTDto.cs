using System;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;

namespace LearnLanguages.Business.Bases
{
  [Serializable]
  public abstract class BusinessBase<C, TDto> : Csla.BusinessBase<C>
    where C : Bases.BusinessBase<C, TDto>
    where TDto : class
  {
    #region Business Properties and Methods

    #region public Guid Id
    public static readonly PropertyInfo<Guid> IdProperty = RegisterProperty<Guid>(c => c.Id);
    public Guid Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }
    #endregion

    public abstract TDto CreateDto();
    public abstract void LoadFromDtoBypassPropertyChecks(TDto dto);
    protected abstract void LoadDefaults();
    protected abstract void LoadDefaults(Guid id);
    protected virtual void SetIdBypassPropertyChecks(Guid id)
    {
      using (BypassPropertyChecks)
      {
        Id = id;
      }
    }

    #endregion

    #region DP_XYZ (All: .net, sl, child)

#if !SILVERLIGHT
    protected override void DataPortal_Create()
    {
      LoadDefaults();
    }
    protected virtual void DataPortal_Create(Guid id)
    {
      LoadDefaults(id);
    }
#endif

#if SILVERLIGHT
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public override void DataPortal_Create(LocalProxy<C>.CompletedHandler handler)
    {
      try
      {
        LoadDefaults();
        handler((C)this, null);
      }
      catch (Exception ex)
      {
        handler(null, ex);
      }
    }
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public virtual void DataPortal_Create(Guid id, LocalProxy<C>.CompletedHandler handler)
    {
      try
      {
        LoadDefaults(id);
        handler((C)this, null);
      }
      catch (Exception ex)
      {
        handler(null, ex);
      }
    }
#endif

    public virtual void Child_Create(TDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);
      BusinessRules.CheckRules();
    }

    #endregion
  }
}
