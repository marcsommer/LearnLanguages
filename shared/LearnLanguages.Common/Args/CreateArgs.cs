using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Args
{
  public class CreateArgs : SampleActionArgs
  {
    public CreateArgs()
      : base()
    {

    }

    public CreateArgs(Guid sampleId, UserInterfaceInfo uiInfo, LearnLanguages.Common.AsynchronousCallback callback)
      : base(sampleId, uiInfo, callback)
    {

    }
  }
}
