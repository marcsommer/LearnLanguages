using System;
using Csla.Core;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOpportunityCompleted<TTarget, TFruit> : IOpportunity
  {
    IOpportunity<TTarget> Opportunity { get; }
    IOffer<TTarget> Offer { get; }
    IOfferResponse<TTarget> AcceptanceOfferResponse { get; }
    IJobInfo<TTarget> JobInfo { get; }
    TFruit Fruit { get; }
  }
}
