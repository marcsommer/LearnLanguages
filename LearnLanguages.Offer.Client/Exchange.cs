using System;
using LearnLanguages.Common.Interfaces;
using Caliburn.Micro;

namespace LearnLanguages.Offer.Client
{
  public class Exchange : IExchange
  {
    #region Singleton Pattern Members
    private static volatile Exchange _Ton;
    private static object _Lock = new object();
    public static Exchange Ton
    {
      get
      {
        if (_Ton == null)
        {
          lock (_Lock)
          {
            if (_Ton == null)
              _Ton = new Exchange();
          }
        }

        return _Ton;
      }
    }
    #endregion

    public Guid ExchangeId
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
