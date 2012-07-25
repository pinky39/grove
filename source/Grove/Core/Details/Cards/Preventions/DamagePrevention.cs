namespace Grove.Core.Details.Cards.Preventions
{
  using Dsl;
  using Infrastructure;
  using Modifiers;
  using Targeting;

  [Copyable]
  public abstract class DamagePrevention : IHashable, ILifetimeDependency
  {
    public Target Owner { get; private set; }
    protected Game Game { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public TrackableEvent EndOfLife { get; set; }

    protected virtual void Initialize() {}

    public virtual void PreventReceivedDamage(Damage damage) {}

    public virtual int PreventLifeloss(int lifeloss)
    {
      return lifeloss;
    }

    public virtual int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return amount;
    }

    public virtual int PreventDealtCombatDamage(int amount)
    {
      return amount;
    }

    public class Factory<T> : IDamagePreventionFactory where T : DamagePrevention, new()
    {
      public Game Game { get; set; }
      public bool OnlyOnce { get; set; }
      public Initializer<T> Init { get; set; }

      public DamagePrevention Create(Target preventionOwner)
      {
        var prevention = new T();

        prevention.Owner = preventionOwner;
        prevention.Game = Game;
        prevention.EndOfLife = new TrackableEvent(prevention, Game.ChangeTracker);

        Init(prevention, new CardBuilder(Game));
        prevention.Initialize();

        return prevention;
      }
    }
  }
}