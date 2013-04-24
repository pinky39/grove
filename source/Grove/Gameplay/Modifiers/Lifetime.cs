namespace Grove.Gameplay.Modifiers
{
  using System;
  using Common;
  using Grove.Infrastructure;

  public abstract class Lifetime : GameObject, IDisposable
  {
    public TrackableEvent Ended { get; set; }
    public Modifier Modifier { get; private set; }

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
      Modifier = modifier;   
      Ended.Initialize(game.ChangeTracker);
    }
  }
}