using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer.Client
{
  [Serializable]
  public class OfferResponse : IOfferResponse
  {
    /// <summary>
    /// Do not use this.  For serialization only.
    /// </summary>
    public OfferResponse()
    {
      
    }

    public OfferResponse(Guid opportunityId, Guid offerId, Guid publisherId, object publisher, string response)
    {
      Id = Guid.NewGuid();
      OpportunityId = opportunityId;
      OfferId = offerId;
      PublisherId = publisherId;
      Publisher = publisher;
      Response = response;
    }

    public Guid Id { get; private set; }
    public Guid OpportunityId { get; private set; }
    public Guid OfferId { get; private set; }
    public Guid PublisherId { get; private set; }
    public object Publisher { get; private set; }
    public string Response { get; private set; }
  }
}
