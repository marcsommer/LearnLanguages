using System;
using Caliburn.Micro;

namespace LearnLanguages.Common.Interfaces
{
  public interface IOfferExchange
  {
    /// <summary>
    /// Id for the individual object (singleton as of this writing) responsible for 
    /// handling offers.
    /// </summary>
    Guid OfferExchangeId { get; }

    /// <summary>
    /// Publish an offer to the exchange.
    /// </summary>
    void Publish(IOffer offer);

    /// <summary>
    /// This is the event aggregator used for responses to offers.  These responses will include:
    /// AcceptOffer, DeclineOffer, and there is always the change of ignoring or timing out offers.
    /// That will be left up to the offerer.  
    /// </summary>
    IEventAggregator OfferResponseEventAggregator { get; }

    ///// <summary>
    ///// May be useful, will see.
    ///// </summary>
    ///// <param name="offer"></param>
    //public void PublishAcceptOfferConfirmed(IOffer offer);

    ///// <summary>
    ///// Should not be necessary I don't think.  Will see.
    ///// </summary>
    ///// <param name="offer"></param>
    //public void PublishOfferCompleted(IOffer offer);
  }
}
