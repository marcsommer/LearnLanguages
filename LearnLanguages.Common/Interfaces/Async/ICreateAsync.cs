using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface ICreateAsync
  {
    Result<IAsyncStub> Create(Args.CreateArgs args);
  }
}
