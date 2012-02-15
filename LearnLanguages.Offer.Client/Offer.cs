using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer.Client
{
  [Serializable]
  public class Offer : IOffer
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public Offer()
    {
    }

    public Offer(Guid opportunityId, Guid publisherId, object publisher, double amount)
    {
      Id = Guid.NewGuid();
      OpportunityId = opportunityId;
      PublisherId = publisherId;
      Publisher = publisher;
      Amount = amount;
    }

    /// <summary>
    /// Id of this offer.
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Opportunity that this offer pertains to.
    /// </summary>
    public Guid OpportunityId { get; private set; }
    /// <summary>
    /// Id of the publisher of this offer.
    /// </summary>
    public Guid PublisherId { get; private set; }
    /// <summary>
    /// Reference to the publisher of this offer.  NOT the exchange.
    /// </summary>
    public object Publisher { get; private set;}
    /// <summary>
    /// Amount offered to do this work.
    /// </summary>
    public double Amount { get; private set; }
  }
}
