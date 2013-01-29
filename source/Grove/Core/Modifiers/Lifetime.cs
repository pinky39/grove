namespace Grove.Core.Modifiers
{
  using System;
  using Infrastructure;

  [Copyable]
  public abstract class Lifetime : GameObject, IDisposable
  {
    public TrackableEvent Ended { get; set; }
    public virtual void Dispose() {}

    protected void End()
    {
      Ended.Raise();
    }

    public void Initialize(Game game)
    {
      Game = game;
      Ended = new TrackableEvent(this, game.ChangeTracker);
    }
  }
}