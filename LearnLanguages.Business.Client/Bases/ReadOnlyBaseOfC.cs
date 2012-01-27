using System;
using System.Net;

namespace LearnLanguages.Business.Bases
{
  public abstract class ReadOnlyBase<C> : Csla.ReadOnlyBase<C>
    where C : Bases.ReadOnlyBase<C>
  {

  }
}
