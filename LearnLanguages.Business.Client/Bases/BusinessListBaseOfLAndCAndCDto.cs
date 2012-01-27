using System;
using Csla.Serialization;

namespace LearnLanguages.Business.Bases
{
  [Serializable]
  public abstract class BusinessListBase<L, C, CDto> :
    Csla.BusinessListBase<L, C>
    where L : Bases.BusinessListBase<L, C, CDto>
    where C : Bases.BusinessBase<C, CDto>
    where CDto : class
  {

  }
}
