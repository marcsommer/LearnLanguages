using System;
using LearnLanguages.Common.Interfaces;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace LearnLanguages.Study.Bases
{
  public class DefaultOfferExchange : IOfferExchange
  {
    public DefaultOfferExchange(IEventAggregator eventAggregator)
    {
      EventAggregator = eventAggregator;
    }

    public Guid OfferExchangeId
    {
      get { return Guid.Parse(StudyResources.DefaultOfferExchangeId); }
    }

    public void Publish(IOffer offer)
    {
      EventAggregator.Publish(offer);
    }

    public IEventAggregator EventAggregator { get; set; }
    public IEventAggregator OfferResponseEventAggregator { get { return EventAggregator; } }
  }
}
