using System;

namespace LearnLanguages.History.CompoundEventMakers
{
  public abstract class CompoundEventMakerBase
  {
    protected abstract void Reset();

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
