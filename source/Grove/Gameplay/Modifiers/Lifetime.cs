namespace Grove.Gameplay.Modifiers
{
  using System;
  using Infrastructure;
  using Misc;

  public abstract class Lifetime : GameObject, IDisposable
  {
    protected Lifetime()
    {
      Ended = new TrackableEvent(this);
    }

    public TrackableEvent Ended { get; set; }
    public Modifier Modifier { get; private set; }
    protected Card OwningCard { get { return Modifier.OwningCard; } }

    public virtual void Dispose() {}

    protected void End()
    {
      Ended.Raise();
    }

    public virtual void Initialize(Game game, Modifier modifier = null)
    {
      Game = game;
      Modifier = modifier;
      Ended.Initialize(game.ChangeTracker);
    }
  }
}