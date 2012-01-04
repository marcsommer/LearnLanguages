using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Interfaces
{
  public interface ILogger
  {
    void Log(string msg, LogPriority priority, LogCategory category);
  }
}
