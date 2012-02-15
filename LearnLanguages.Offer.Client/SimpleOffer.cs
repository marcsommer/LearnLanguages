using System;
using LearnLanguages.Common.Interfaces;
using Csla.Serialization;

namespace LearnLanguages.Offer.Client
{
  [Serializable]
  public class SimpleOffer : IOffer
  {
    /// <summary>
    /// For serialization, do not use.
    /// </summary>
    public SimpleOffer()
    {
    }

    public SimpleOffer(Guid originatorId, object originator, double amount)
    {
      Id = Guid.NewGuid();
      OriginatorId = originatorId;
      Originator = originator;
      Amount = amount;
    }

    public Guid Id { get; private set; }
    public Guid OriginatorId { get; private set; }
    public object Originator { get; private set;}
    public double Amount { get; private set; }
  }
}
