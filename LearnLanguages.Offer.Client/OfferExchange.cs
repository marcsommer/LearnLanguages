using System;
using LearnLanguages.Common.Interfaces;
using Caliburn.Micro;

namespace LearnLanguages.Offer.Client
{
  public class OfferExchange : IOfferExchange
  {
    #region Singleton Pattern Members
    private static volatile OfferExchange _Ton;
    private static object _Lock = new object();
    public static OfferExchange Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new OfferExchange();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public Guid OfferExchangeId
    {
      get { return Guid.Parse(OfferResources.OfferExchangeId); }
    }

    public void Publish(IOffer offer)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Right now, this is just Services.EventAggregator.
    /// </summary>
    public IEventAggregator OfferResponseEventAggregator
    {
      get { return Services.EventAggregator; }
    }
  }
}
