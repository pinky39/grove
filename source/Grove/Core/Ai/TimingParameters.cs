namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using Effects;

  public class TimingParameters
  {
    public TimingParameters(Game game, Card card, ActivationParameters activation = null, bool targetsSelf = false)
    {
      TargetsSelf = targetsSelf;
      Game = game;
      Card = card;
      Activation = activation ?? ActivationParameters.Default;
    }

    public bool TargetsSelf { get; private set; }

    public Game Game { get; private set; }
    public Card Card { get; private set; }
    public ActivationParameters Activation { get; private set; }
    public Step Step { get { return Game.Turn.Step; } }
    public Player Controller { get { return Card.Controller; } }
    public Player Opponent { get { return Game.Players.GetOpponent(Controller); } }
    public Effect TopSpell { get { return Game.Stack.TopSpell; } }
    public Player TopSpellController { get { return TopSpell == null ? null : TopSpell.Controller; } }
    public IEnumerable<Attacker> Attackers { get { return Game.Combat.Attackers; } }
    public bool IsAttached { get { return Card.IsAttached; } }
    public ITarget Target { get { return Activation.Target; } }

    public bool IsCannonfodder()
    {
      return Game.Combat.IsBlockerThatWillBeDealtLeathalDamageAndWillNotKillAttacker(Card);
    }

    public bool CanThisBeDestroyedByTopSpell()
    {
      if (TopSpell == null)
        return false;

      if (!Card.CanBeDestroyed)
        return false;

      if (TopSpell.HasCategory(EffectCategories.Destruction))
      {
        if (!TopSpell.HasTarget)
          return true;

        return TopSpell.Target == Card;
      }

      var damageDealing = TopSpell as IDamageDealing;

      if (damageDealing == null)
        return false;

      var damage = new Damage(TopSpell.Source.OwningCard, damageDealing.CreatureDamage(Card));
      var dealtAmount = Card.CalculateDealtDamageAmount(damage);

      return damage.IsLeathal || Card.LifepointsLeft <= dealtAmount;
    }

    public bool CanThisBeDealtLeathalCombatDamage()
    {
      return Game.Combat.CanBeDealtLeathalCombatDamage(Card);
    }

    public bool TopSpellCategoryIs(EffectCategories effectCategories)
    {
      return TopSpell != null && TopSpell.HasCategory(effectCategories);
    }

    public bool TopSpellCanAffectThis()
    {
      return TopSpell.Target == null || TopSpell.Target == Target || TopSpell.Target == Card;
    }

    public bool IsAttackerWithoutBlockersOrIsAttackerWithTrample(Card card)
    {
      return card.IsAttacker && (!Game.Combat.HasBlockers(card) || card.Has().Trample);
    }

    public bool CanBlockAnyAttacker(Card card)
    {
      return Game.Combat.CanBlockAnyAttacker(card);
    }
  }
}