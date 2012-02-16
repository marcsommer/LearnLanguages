using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IWorkStatusUpdate<T> : IWorkStatusUpdate
  {
    IOpportunity<T> Opportunity { get; }
    IOffer<T> Offer { get; }
    IOfferResponse<T> OfferResponse { get; }
    IJobInfo<T> JobInfo { get; }
  }
}
