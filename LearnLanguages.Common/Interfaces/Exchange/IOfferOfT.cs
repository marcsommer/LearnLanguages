using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOffer<T> : IOffer
  {
    IOpportunity<T> Opportunity { get; }
    double Amount { get; }
  }
}
