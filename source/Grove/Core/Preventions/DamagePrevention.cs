namespace Grove.Core.Preventions
{
  using CardDsl;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public abstract class DamagePrevention : IHashable, ILifetimeDependency
  {
    protected ITarget Owner { get; private set; }
    protected Game Game { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public TrackableEvent EndOfLife { get; set; }

    protected virtual void Initialize() {}

    public abstract int PreventDamage(Card damageDealer, int damageAmount, bool queryOnly);

    public class Factory<T> : IDamagePreventionFactory where T : DamagePrevention, new()
    {
      public Game Game { get; set; }
      public bool OnlyOnce { get; set; }
      public Initializer<T> Init { get; set; }

      public DamagePrevention Create(ITarget owner)
      {
        var prevention = new T();

        prevention.Owner = owner;
        prevention.Game = Game;
        prevention.EndOfLife = new TrackableEvent(prevention, Game.ChangeTracker);

        Init(prevention, new CardCreationContext(Game));

        return prevention;
      }
    }
  }
}