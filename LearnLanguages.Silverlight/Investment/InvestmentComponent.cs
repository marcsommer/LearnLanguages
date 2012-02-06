using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Silverlight.Investment
{
  public class InvestmentComponent
  {
    public Guid Id { get; set; }
    public Guid InvestmentSessionId { get; set; }
    public Guid TargetId { get; set; }
    public double InvestmentWeight { get; set; }
    public InvestmentAction InvestmentAction { get; set; }
    public double[] Features { get; set; }
  }
}
