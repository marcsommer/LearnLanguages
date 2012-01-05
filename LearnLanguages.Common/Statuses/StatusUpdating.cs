using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Statuses
{
  public class StatusUpdating : Status
  {
    public override string Value()
    {
      return CommonResources.StatusUpdating;
    }
  }
}
