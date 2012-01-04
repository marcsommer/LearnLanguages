using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface IEditAsync
  {
    Result<IAsyncStub> Edit(Args.EditArgs args);
  }
}
