namespace Grove.Gameplay.Damage
{
  using System;
  using Infrastructure;
  using Misc;
  using Modifiers;
  using Targeting;

  [Copyable]
  public abstract class DamagePrevention : GameObject, IHashable, ILifetimeDependency
  {
    protected DamagePrevention()
    {
      EndOfLife = new TrackableEvent(this);
    }

    public ITarget Owner { get; private set; }

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

    public virtual void Initialize(ITarget owner, Game game)
    {
      Game = game;
      Owner = owner;
      EndOfLife.Initialize(game.ChangeTracker);
    }

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

    public virtual int EvaluateDealtCombatDamage(int amount)
    {
      return amount;
    }
  }
}