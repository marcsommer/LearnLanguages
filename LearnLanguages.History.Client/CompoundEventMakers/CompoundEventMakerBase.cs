using System;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.History.CompoundEventMakers
{
  public abstract class CompoundEventMakerBase : ICompoundEventMaker
  {
    protected abstract void Reset();

    public Guid Id { get { return GetId(); } }

    protected abstract Guid GetId();

    public virtual void Enable()
    {
      Reset();
      HistoryPublisher.Ton.SubscribeToEvents(this);
    }

    public virtual void Disable()
    {
      HistoryPublisher.Ton.UnsubscribeFromEvents(this);
      Reset();
    }
  }
}
