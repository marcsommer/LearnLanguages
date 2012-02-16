using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class WorkStatusUpdate<T> : IWorkStatusUpdate<T>
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public WorkStatusUpdate()
    {
    }

    public WorkStatusUpdate(IOpportunity<T> opportunity, Guid publisherId, object publisher, double amount,
      string category, object information)
    {
      Id = Guid.NewGuid();
      Opportunity = opportunity;
      PublisherId = publisherId;
      Publisher = publisher;
      Amount = amount;
      Category = category;
      Information = information;
    }

    /// <summary>
    /// Id of this offer.
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Opportunity that this offer pertains to.
    /// </summary>
    public IOpportunity<T> Opportunity { get; private set; }
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
    /// <summary>
    /// Any additional information about the offer.
    /// </summary>
    public object Information { get; private set; }
    /// <summary>
    /// Category of the offer.  E.g. Study
    /// </summary>
    public string Category { get; private set; }




    public IOffer<T> Offer
    {
      get { throw new NotImplementedException(); }
    }

    public IOfferResponse<T> OfferResponse
    {
      get { throw new NotImplementedException(); }
    }

    public IJobInfo<T> JobInfo
    {
      get { throw new NotImplementedException(); }
    }

    public Statuses.Status Status
    {
      get { throw new NotImplementedException(); }
    }
  }
}
