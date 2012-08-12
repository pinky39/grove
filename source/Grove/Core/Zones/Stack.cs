namespace Grove.Core.Zones
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Details.Cards.Effects;
  using Infrastructure;

  [Copyable]
  public class Stack : IEnumerable<Effect>, IHashable, IZone
  {
    private readonly TrackableList<Effect> _effects;
    private readonly Trackable<Effect> _lastResolved;

    public Stack(ChangeTracker changeTracker)
    {
      _effects = new TrackableList<Effect>(changeTracker, orderImpactsHashcode: true);
      _lastResolved = new Trackable<Effect>(changeTracker);
    }

    private Stack()
    {
      /* for state copy */
    }

    public int Count { get { return _effects.Count; } }
    public bool IsEmpty { get { return _effects.Count == 0; } }

    public Effect LastResolved { get { return _lastResolved.Value; } private set { _lastResolved.Value = value; } }
    public Effect TopSpell { get { return _effects.LastOrDefault(); } }
    public Player TopSpellOwner { get { return TopSpell == null ? null : TopSpell.Controller; } }

    public IEnumerator<Effect> GetEnumerator()
    {
      return _effects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_effects);
    }

    public Zone Zone { get { return Zone.Stack; } }

    public void Remove(Card card) {}

    public void AfterRemove(Card card) {}

    public void AfterAdd(Card card) {}

    public bool HasOnlySpellsOwnedBy(Player player)
    {
      return _effects.All(x => x.Controller == player);
    }

    public void Push(Effect effect)
    {
      Add(effect);
      effect.EffectWasPushedOnStack();
    }

    public void Resolve()
    {
      LastResolved = null;

      var effect = TopSpell;
      Remove(effect);

      if (effect.CanBeResolved())
      {
        effect.Resolve();
      }

      LastResolved = effect;
    }

    public override string ToString()
    {
      return String.Format("{0}", Count);
    }

    protected virtual void Add(Effect effect)
    {
      _effects.Add(effect);
    }

    protected virtual void Remove(Effect effect)
    {
      _effects.Remove(effect);
    }

    public void Counter(Effect effect)
    {
      Remove(effect);
      effect.EffectWasCountered();
    }

    public int GetDamageTopSpellWillDealToPlayer(Player player)
    {
      if (TopSpell == null)
        return 0;

      return TopSpell.CalculatePlayerDamage(player);
    }

    public int GetDamageTopSpellWillDealToCreature(Card card, bool targetOnly = false)
    {
      if (TopSpell == null)
        return 0;

      if (!TopSpell.HasTargets && targetOnly)
        return 0;


      var dealtAmount = card.EvaluateReceivedDamage(
        TopSpell.Source.OwningCard, TopSpell.CalculateCreatureDamage(card), isCombat: false);

      return dealtAmount;
    }

    public bool CanBeDealtLeathalDamageByTopSpell(Card card, bool targetOnly = false)
    {
      if (!card.Is().Creature)
        return false;

      var dealtAmount = GetDamageTopSpellWillDealToCreature(card, targetOnly);
      return card.Life <= dealtAmount;
    }

    public bool CanBeDestroyedByTopSpell(Card card, bool targetOnly = false)
    {
      if (IsEmpty)
        return false;

      if (!card.CanBeDestroyed)
        return false;


      if (TopSpell.HasCategory(EffectCategories.Destruction))
      {
        if (!TopSpell.HasTargets)
          return !targetOnly;

        return TopSpell.HasTarget(card);
      }

      if (card.Is().Creature == false)
        return false;

      return CanThougnessBeReducedToLeathalByTopSpell(card) || CanBeDealtLeathalDamageByTopSpell(card);
    }

    private bool CanThougnessBeReducedToLeathalByTopSpell(Card card)
    {
      if (TopSpell == null)
        return false;

      return card.Life <=
        TopSpell.CalculateToughnessReduction(card);
    }

    public bool CanTopSpellReducePlayersLifeToZero(Player player)
    {
      if (TopSpell == null)
        return false;

      var damage = TopSpell.CalculatePlayerDamage(player);
      return damage >= player.Life;
    }
  }
}