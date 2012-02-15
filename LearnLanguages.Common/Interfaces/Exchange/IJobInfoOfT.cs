using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IJobInfo<TTarget>
  {
    Guid Id { get; }
    TTarget Target { get; }
    DateTime JobExpirationDate { get; }
    IExchange OfferExchange { get; }
    double ExpectedPrecision { get; }
  }
}
