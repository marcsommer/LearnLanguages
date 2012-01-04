using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface IDeleteAsync
  {
    Result<IAsyncStub> Delete(Args.DeleteArgs args);
  }
}
