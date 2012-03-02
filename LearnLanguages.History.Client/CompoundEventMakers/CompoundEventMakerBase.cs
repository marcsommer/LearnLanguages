using System;

namespace LearnLanguages.History.CompoundEventMakers
{
  public class CompoundEventMakerBase
  {
    protected abstract void Reset();

    public virtual void Enable()
    {
      Reset();
      HistoryPublisher.Ton.Subscribe(this);
    }

    public virtual void Disable()
    {
      HistoryPublisher.Ton.Unsubscribe(this);
      Reset();
    }
  }
}
