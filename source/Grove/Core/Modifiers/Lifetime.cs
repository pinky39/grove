namespace Grove.Core.Modifiers
{
  using System;
  using Infrastructure;

  [Copyable]
  public abstract class Lifetime : IDisposable
  {
    private readonly Modifier _modifier;
    public TrackableEvent Ended { get; set; }

    protected Lifetime(Modifier modifier, ChangeTracker changeTracker)
    {
      Ended = new TrackableEvent(this, changeTracker);
      _modifier = modifier;
    }

    protected Lifetime() {/* for state copy */}        

    protected Card ModifierSource
    {
      get { return _modifier.Source; }
    }

    protected Card ModifierTarget
    {
      get { return _modifier.Target; }
    }

    protected void End()
    {
      Ended.Raise();
    }

    public virtual void Dispose()
    {      
    }       
  }
}