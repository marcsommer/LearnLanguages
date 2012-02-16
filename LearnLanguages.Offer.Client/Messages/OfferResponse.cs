using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer
{
  [Serializable]
  public class OfferResponse<T> : IOfferResponse<T>
  {
    /// <summary>
    /// Do not use this.  For serialization only.
    /// </summary>
    public OfferResponse()
    {
      
    }

    public OfferResponse(Offer<T> offer, Guid publisherId, object publisher, 
      string response, string category, object information)
    {
      Id = Guid.NewGuid();
      Offer = offer;
      PublisherId = publisherId;
      Publisher = publisher;
      Response = response;
      Category = category;
      Information = information;
    }

    public Guid Id { get; private set; }
    public Offer<T> Offer { get; private set; }
    public Guid PublisherId { get; private set; }
    public object Publisher { get; private set; }
    public string Response { get; private set; }
    /// <summary>
    /// Any additional information about the offer.
    /// </summary>
    public object Information { get; private set; }
    /// <summary>
    /// Category of the offer.  E.g. Study
    /// </summary>
    public string Category { get; private set; }
  }
}
