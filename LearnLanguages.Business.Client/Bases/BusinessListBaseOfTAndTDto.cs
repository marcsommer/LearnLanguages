using System;
using Csla.Serialization;

namespace LearnLanguages.Business.Bases
{
  [Serializable]
  public abstract class BusinessListBase<T, C, TDto> :
    Csla.BusinessListBase<T, C>
    where T : Bases.BusinessListBase<T, C, TDto>
    where C : Bases.BusinessBase<C, TDto>
    where TDto : class
  {

  }
}
