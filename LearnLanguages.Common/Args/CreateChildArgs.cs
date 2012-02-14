using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Args
{
  public class CreateChildArgs : SampleActionArgs
  {
    public CreateChildArgs()
      : base()
    {
       
    }

    public CreateChildArgs(Guid sampleId, UserInterfaceInfo uiInfo, LearnLanguages.Common.Delegates.AsynchronousCallback callback)
      : base(sampleId, uiInfo, callback)
    {

    }
  }
}
