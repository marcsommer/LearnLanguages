using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOfferResponse<T> : IOfferResponse
  {
    /// <summary>
    /// Id of the offer this response pertains to.
    /// </summary>
    IOffer<T> Offer { get; }

    /// <summary>
    /// Actual response.  Check CommonResources.OfferResponse_______ (e.g. CommonResources.OfferResponseAccept)
    /// </summary>
    string Response { get; }
  }
}
