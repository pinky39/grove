namespace Grove.Gameplay.Zones
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Artifical;
  using Effects;
  using Infrastructure;
  using Targeting;

  [Copyable, Serializable]
  public class Stack : IEnumerable<Effect>, IHashable, IZone
  {
    private readonly TrackableList<Effect> _effects = new TrackableList<Effect>(orderImpactsHashcode: true);
    private readonly Trackable<Effect> _lastResolved = new Trackable<Effect>();

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
    public void AfterAdd(Card card) {}
    public void AfterRemove(Card card) {}

    public void Initialize(Game game)
    {
      _effects.Initialize(game.ChangeTracker);
      _lastResolved.Initialize(game.ChangeTracker);
    }

    public event EventHandler<StackChangedEventArgs> EffectAdded = delegate { };
    public event EventHandler<StackChangedEventArgs> EffectRemoved = delegate { };

    public bool HasOnlySpellsOwnedBy(Player player)
    {
      return _effects.All(x => x.Controller == player);
    }

    public void Push(Effect effect)
    {
      _effects.Add(effect);
      effect.EffectWasPushedOnStack();

      EffectAdded(this, new StackChangedEventArgs(effect));

      LogFile.Debug("Effect pushed on stack: {0}.", effect);
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

    public IEnumerable<ITarget> GenerateTargets(Func<Zone, Player, bool> zoneFilter)
    {
      if (zoneFilter(Zone, null))
      {
        foreach (var effect in _effects)
        {
          yield return effect;
        }
      }

      yield break;
    }

    private void Remove(Effect effect)
    {
      _effects.Remove(effect);
      EffectRemoved(this, new StackChangedEventArgs(effect));

      LogFile.Debug("Effect removed from stack: {0}. (count: {1})", effect, _effects.Count);
    }

    public void Counter(Effect effect)
    {
      Remove(effect);
      effect.EffectCountered(SpellCounterReason.SpellOrAbility);
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

      if (!TopSpell.Targets.Any() && targetOnly)
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

    public bool CanBeBouncedByTopSpell(Card card)
    {
      if (IsEmpty)
        return false;

      if (TopSpell.HasCategory(EffectCategories.Bounce))
      {
        return TopSpell.HasTarget(card);
      }

      return false;
    }

    public bool CanBeDestroyedByTopSpell(Card card, bool targetOnly = false)
    {
      if (IsEmpty)
        return false;

      if (!card.CanBeDestroyed)
        return false;

      if (TopSpell.HasCategory(EffectCategories.Destruction))
      {
        if (!TopSpell.Targets.Any())
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

    public bool HasSpellWithSource(Card card)
    {
      return _effects.Any(x => x.Source.OwningCard == card);
    }
    
    public bool HasSpellWithName(string name)
    {
      return _effects.Any(x => x.Source.OwningCard.Name.Equals(name));
    }
  }
}