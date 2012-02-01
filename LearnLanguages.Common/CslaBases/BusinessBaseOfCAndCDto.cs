using System;
using Csla;
using Csla.Serialization;
using Csla.DataPortalClient;

namespace LearnLanguages.Common.CslaBases
{
  [Serializable]
  public abstract class BusinessBase<C, CDto> : Csla.BusinessBase<C>
    where C : CslaBases.BusinessBase<C, CDto>
    where CDto : class
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

    public abstract CDto CreateDto();
    public virtual void LoadFromDtoBypassPropertyChecks(CDto dto)
    {
      LoadFromDtoBypassPropertyChecksImpl(dto);
      BusinessRules.CheckRules();
    }
    public abstract void LoadFromDtoBypassPropertyChecksImpl(CDto dto);

    protected virtual void SetIdBypassPropertyChecks(Guid id)
    {
      using (BypassPropertyChecks)
      {
        Id = id;
      }
    }

    #endregion

    #region DP_XYZ (All: .net, sl, child)

    public virtual void Child_Create(CDto dto)
    {
      LoadFromDtoBypassPropertyChecks(dto);
    }

    #endregion
  }
}
