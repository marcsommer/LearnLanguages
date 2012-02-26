using System;
using System.Collections.Generic;
using LearnLanguages.Business;
using Csla.Serialization;

namespace LearnLanguages.History.Bases
{
  [Serializable]
  public abstract class SingleLineEventBase : HistoryEventBase
  {
    public SingleLineEventBase()
      : base()
    {

    }

    public SingleLineEventBase(LineEdit Line)
      : base(TimeSpan.MinValue,
             new KeyValuePair<string, object>(HistoryResources.Key_LineId, Line.Id)) 
    {

    }

    public SingleLineEventBase(LineEdit Line, TimeSpan duration)
      : base(duration,
             new KeyValuePair<string, object>(HistoryResources.Key_LineId, Line.Id))
    {

    }

    public SingleLineEventBase(LineEdit Line, params KeyValuePair<string, object>[] details)
      : base(TimeSpan.MinValue,
             new KeyValuePair<string, object>(HistoryResources.Key_LineId, Line.Id))
    {
      AddDetails(details);
    }

    public SingleLineEventBase(LineEdit Line, TimeSpan duration, params KeyValuePair<string, object>[] details)
      : base(duration,
             new KeyValuePair<string, object>(HistoryResources.Key_LineId, Line.Id))
    {
      AddDetails(details);
    }
  }
}
