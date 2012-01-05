using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Args
{
  public class DeleteArgs : SampleActionArgs
  {
    public DeleteArgs()
      : base()
    {
       
    }

    public DeleteArgs(Guid sampleId, LearnLanguages.Common.AsynchronousCallback callback)
    {
      this.SampleId = sampleId;
      this.Callback = callback;
    }
    //public DeleteArgs(Guid sampleId, UserInterfaceInfo uiInfo)
    //  : base(sampleId, uiInfo)
    //{

    //}
  }
}
