using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOffer : IExchangeMessage
  {
    Guid OpportunityId { get; }
    double Amount { get; }
  }
}
