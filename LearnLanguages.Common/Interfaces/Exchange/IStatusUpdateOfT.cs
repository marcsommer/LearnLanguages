using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IStatusUpdate<T> : IStatusUpdate
  {
    IOpportunity<T> Opportunity { get; }
    IOffer<T> Offer { get; }
    IOfferResponse<T> AcceptanceOfferResponse { get; }
    IJobInfo<T> JobInfo { get; }
  }
}
