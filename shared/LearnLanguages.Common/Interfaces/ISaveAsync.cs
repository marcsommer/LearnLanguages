using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface ISaveAsync
  {
    Result<IAsyncStub> Save(Args.SaveArgs args);
  }
}
