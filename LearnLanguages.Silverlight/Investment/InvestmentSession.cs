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

namespace LearnLanguages.Silverlight.Investment
{
  public class InvestmentSession
  {
    public Guid Id { get; set;  }
    ICollection<Guid> InvestmentComponentIds { get; set; }
    DateTime StartTime { get; set; }
    int DurationInMs { get; set; }
    bool Aborted { get; set; }
    double Score { get; set;}
  }
}
