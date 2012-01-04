using System;
using Csla;
using Csla.Serialization;

namespace LearnLanguages.Business.Bases
{
  [Serializable]
  public abstract class BusinessBase<C, TDto> : Csla.BusinessBase<C>
      where C : Bases.BusinessBase<C, TDto>
  {
    public abstract TDto CreateDto();
    public abstract void LoadFromDtoBypassPropertyChecks(TDto dto);

    #region public Guid Id
    public static readonly PropertyInfo<Guid> IdProperty = RegisterProperty<Guid>(c => c.Id);
    public Guid Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }
    #endregion
  }
}
