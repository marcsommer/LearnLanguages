using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOffer
  {
    Guid Id { get; }
    Guid OpportunityId { get; }
    Guid OriginatorId { get; }
    object Originator { get; }
    double Amount { get; }
  }
}
