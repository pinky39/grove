namespace Grove.Core.Details.Cards.Modifiers
{
  using System;
  using Infrastructure;
  using Targeting;

  [Copyable]
  public abstract class Lifetime : IDisposable
  {
    private readonly Modifier _modifier;

    protected Lifetime(Modifier modifier, ChangeTracker changeTracker)
    {
      Ended = new TrackableEvent(this, changeTracker);
      _modifier = modifier;
    }

    protected Lifetime()
    {
/* for state copy */
    }

    public TrackableEvent Ended { get; set; }

    protected Card ModifierSource { get { return _modifier.Source; } }

    protected ITarget ModifierTarget { get { return _modifier.Target; } }

    public virtual void Dispose() {}

    protected void End()
    {
      Ended.Raise();
    }
  }
}