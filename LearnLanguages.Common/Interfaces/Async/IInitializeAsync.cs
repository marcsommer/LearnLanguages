using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface IInitializeAsync
  {
    Result<IAsyncStub> Initialize(Args.InitializeArgs args);
  }
}
