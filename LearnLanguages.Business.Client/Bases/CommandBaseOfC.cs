using System;
using System.Net;

namespace LearnLanguages.Business.Bases
{
  public abstract class CommandBase<C> : Csla.CommandBase<C>
    where C : Bases.CommandBase<C>
  {

  }
}
