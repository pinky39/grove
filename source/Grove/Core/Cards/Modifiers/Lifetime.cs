namespace Grove.Core.Cards.Modifiers
{
  using System;
  using Grove.Core.Dsl;
  using Grove.Infrastructure;

  [Copyable]
  public abstract class Lifetime : IDisposable
  {
    protected Lifetime() {}
    
    public TrackableEvent Ended { get; set; }

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
        lifetime.Ended = new TrackableEvent(this, game.ChangeTracker);

        Init(lifetime);

        return lifetime;
      }
    }
  }
}