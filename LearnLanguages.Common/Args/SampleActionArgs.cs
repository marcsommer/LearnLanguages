using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Common.Args
{
  public class SampleActionArgs
  {
    public SampleActionArgs()
    {

    }

    public SampleActionArgs(Guid sampleId, UserInterfaceInfo uiInfo, Delegates.AsynchronousCallback callback)
    {
      this.SampleId = sampleId;
      this.UserInterfaceInfo = uiInfo;
      this.Callback = callback;
    }

    public Guid SampleId { get; set; }
    public UserInterfaceInfo UserInterfaceInfo { get; set; }
    public Delegates.AsynchronousCallback Callback { get; set; }
  }
}
