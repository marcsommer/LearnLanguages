using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Args
{
  public class SaveArgs : SampleActionArgs
  {
    public SaveArgs()
      : base()
    {
       
    }

    public SaveArgs(Guid sampleId, UserInterfaceInfo uiInfo, LearnLanguages.Common.AsynchronousCallback callback)
      : base(sampleId, uiInfo, callback)
    {

    }
  }
}
