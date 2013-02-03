namespace Grove.Core.Modifiers
{
  using System;
  using Infrastructure;

  [Copyable]
  public abstract class Lifetime : GameObject, IDisposable
  {
    public TrackableEvent Ended;

    protected Lifetime()
    {
      Ended = new TrackableEvent(this);
    }

    public virtual void Dispose() {}

    protected void End()
    {
      Ended.Raise();
    }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Game = game;            
      Ended.Initialize(game);
    }
  }
}