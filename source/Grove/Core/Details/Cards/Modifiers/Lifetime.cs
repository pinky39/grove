namespace Grove.Core.Details.Cards.Modifiers
{
  using System;
  using Infrastructure;

  [Copyable]
  public abstract class Lifetime : IDisposable
  {
    protected Lifetime(ChangeTracker changeTracker)
    {
      Ended = new TrackableEvent(this, changeTracker);
    }

    protected Lifetime()
    {
      // for state copy //
    }

    public TrackableEvent Ended { get; set; }

    public virtual void Dispose() {}

    protected void End()
    {
      Ended.Raise();
    }
  }
}