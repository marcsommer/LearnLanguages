using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public class MultiLineTextMeaningInfo
  {
    public MultiLineTextMeaningInfo(MultiLineTextEdit multiLineTextEdit, int originalAggregateSize)
    {
      MultiLineTextEdit = multiLineTextEdit;
      ActiveLines = new Dictionary<int, DefaultLineMeaningStudier>();
      foreach (var line in multiLineTextEdit.Lines)
      {
        DefaultLineMeaningStudier lineInfo = new DefaultLineMeaningStudier(line, originalAggregateSize);
        ActiveLines.Add(lineInfo.Line.LineNumber, lineInfo);
      }
    }

    public MultiLineTextEdit MultiLineTextEdit { get; private set; }
    /// <summary>
    /// These lines are actively being studied using the aggregating system.  Their percent known varies from
    /// line to line.  Once they are known, they should be removed from this list and placed in the KnownLines 
    /// list.
    /// </summary>
    public Dictionary<int, DefaultLineMeaningStudier> ActiveLines { get; private set; }
  }
}
