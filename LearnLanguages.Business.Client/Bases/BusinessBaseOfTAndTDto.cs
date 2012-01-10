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

    protected virtual void SetIdBypassPropertyChecks(Guid id)
    {
      using (BypassPropertyChecks)
      {
        Id = id;
      }
    }

    #endregion

    #region DP_XYZ (All: .net, sl, child)


    public virtual void Child_Create(TDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);
      BusinessRules.CheckRules();
    }

    #endregion
  }
}
