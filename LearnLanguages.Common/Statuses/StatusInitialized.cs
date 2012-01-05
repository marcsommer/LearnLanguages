using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Statuses
{
  public class StatusInitialized : Status
  {
    public override string Value()
    {
      return CommonResources.StatusInitialized;
    }
  }
}
