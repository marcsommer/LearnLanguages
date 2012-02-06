using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLanguages.Silverlight.Interfaces
{
  public class InvestmentMethod
  {
    public Guid Id { get; set; }
    public string Text { get; set; }
    ICollection<Guid> SessionIds { get; set; }
  }
}
