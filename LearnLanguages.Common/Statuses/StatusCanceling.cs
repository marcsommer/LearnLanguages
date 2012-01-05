using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Statuses
{
  public class StatusCanceling : Status
  {
    public override string Value()
    {
      return CommonResources.StatusCanceling;
    }
  }
}
