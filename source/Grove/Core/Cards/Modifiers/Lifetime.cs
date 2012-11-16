namespace Grove.Core.Cards.Modifiers
{
  using System;
  using Dsl;
  using Infrastructure;

  [Copyable]
  public abstract class Lifetime : IDisposable
  {
    public TrackableEvent Ended { get; set; }
    protected Game Game { get; private set; }

    public virtual void Dispose() {}

    protected void End()
    {
      Ended.Raise();
    }

    public class Factory<TLifetime> : ILifetimeFactory where TLifetime : Lifetime, new()
    {
      public Initializer<TLifetime> Init = delegate { };

      public Lifetime CreateLifetime(Game game)
      {
        var lifetime = new TLifetime();
        lifetime.Game = game;
        lifetime.Ended = new TrackableEvent(this, game.ChangeTracker);

        Init(lifetime);

        return lifetime;
      }
    }
  }
}