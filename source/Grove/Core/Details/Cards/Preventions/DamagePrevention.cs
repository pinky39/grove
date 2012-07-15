namespace Grove.Core.Details.Cards.Preventions
{
  using Dsl;
  using Infrastructure;
  using Modifiers;
  using Targeting;

  [Copyable]
  public abstract class DamagePrevention : IHashable, ILifetimeDependency
  {
    public ITarget Owner { get; private set; }
    protected Game Game { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public TrackableEvent EndOfLife { get; set; }

    protected virtual void Initialize() {}

    public virtual void PreventDamage(Damage damage) {}
    
    public virtual int PreventLifeloss(int lifeloss)
    {
      return lifeloss;
    }

    public abstract int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat);

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
        prevention.Initialize();

        return prevention;
      }
    }
  }
}