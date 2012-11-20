namespace Grove.Core.Cards.Preventions
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
    protected CardBuilder Builder { get { return new CardBuilder(); } }

    public Player Controller
    {
      get
      {
        return Owner.IsCard()
          ? Owner.Card().Controller
          : Owner.Player();
      }
    }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public TrackableEvent EndOfLife { get; set; }

    protected virtual void Initialize() {}

    public virtual void PreventReceivedDamage(Damage damage)
    {
      damage.Amount = EvaluateReceivedDamage(damage.Source, damage.Amount, damage.IsCombat);       
    }

    public virtual int PreventLifeloss(int lifeloss)
    {
      return lifeloss;
    }

    public virtual int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      return amount;
    }

    public virtual int PreventDealtDamage(int amount)
    {
      return amount;
    }

    public class Factory<T> : IDamagePreventionFactory where T : DamagePrevention, new()
    {
      public bool OnlyOnce { get; set; }
      public Initializer<T> Init { get; set; }

      public DamagePrevention Create(ITarget preventionOwner, Game game)
      {
        var prevention = new T();

        prevention.Owner = preventionOwner;
        prevention.Game = game;
        prevention.EndOfLife = new TrackableEvent(prevention, game.ChangeTracker);

        Init(prevention);
        prevention.Initialize();

        return prevention;
      }
    }
  }
}