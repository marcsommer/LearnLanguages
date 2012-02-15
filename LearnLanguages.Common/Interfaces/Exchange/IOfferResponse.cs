using System;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOfferResponse
  {
    /// <summary>
    /// Id of this OfferResponse.
    /// </summary>
    Guid Id { get; }
    /// <summary>
    /// Id of the opportunity this response pertains to.
    /// </summary>
    Guid OpportunityId { get; }
    /// <summary>
    /// Id of the offer this response pertains to.
    /// </summary>
    Guid OfferId { get; }
    /// <summary>
    /// Id of of the publisher of this OfferResponse
    /// </summary>
    Guid PublisherId { get; }
    /// <summary>
    /// Reference to publisher of this OfferResponse
    /// </summary>
    object Publisher { get; }
    /// <summary>
    /// Actual response.  Check CommonResources.OfferResponse_______ (e.g. CommonResources.OfferResponseAccept)
    /// </summary>
    string Response { get; }
  }
}
