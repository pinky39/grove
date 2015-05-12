namespace Grove
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;
  using Effects;
  using Events;
  using Infrastructure;

  public class Stack : GameObject, IEnumerable<Effect>, IHashable, IZone
  {
    private readonly TrackableList<Effect> _effects = new TrackableList<Effect>(orderImpactsHashcode: true);
    private readonly Trackable<Effect> _lastResolved = new Trackable<Effect>();
    private readonly TrackableList<Effect> _triggeredEffects = new TrackableList<Effect>();

    public int Count { get { return _effects.Count; } }
    public bool IsEmpty { get { return _effects.Count == 0; } }

    private Effect LastResolved { get { return _lastResolved.Value; } set { _lastResolved.Value = value; } }
    public Effect TopSpell { get { return _effects.LastOrDefault(); } }
    public Player TopSpellOwner { get { return TopSpell == null ? null : TopSpell.Controller; } }
    public bool HasTriggered { get { return _triggeredEffects.Count > 0; } }

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

    public Zone Name { get { return Zone.Stack; } }
    public void Remove(Card card) {}    

    public void Initialize(Game game)
    {
      Game = game;

      _effects.Initialize(ChangeTracker);
      _lastResolved.Initialize(ChangeTracker);
      _triggeredEffects.Initialize(ChangeTracker);
    }

    public bool HasOnlySpellsOwnedBy(Player player)
    {
      return _effects.All(x => x.Controller == player);
    }

    public void QueueTriggered(Effect effect)
    {
      _triggeredEffects.Add(effect);
    }

    public void PushTriggered()
    {
      if (_triggeredEffects.Count == 0)
        return;
      
      var active = _triggeredEffects
        .Where(x => x.Controller == Players.Active)
        .ToList();

      var passive = _triggeredEffects
        .Where(x => x.Controller == Players.Passive)
        .ToList();

      _triggeredEffects.Clear();

      if (active.Count > 0)
      {
        Enqueue(new PushTriggeredEffects(
          Players.Active,
          active));
      }

      if (passive.Count > 0)
      {
        Enqueue(new PushTriggeredEffects(
          Players.Passive,
          passive));
      }
    }

    public void Push(Effect effect)
    {
      _effects.Add(effect);
      effect.EffectWasPushedOnStack();

      EffectAdded(this, new StackChangedEventArgs(effect));
      Publish(new EffectPutOnStackEvent(effect));
      LogFile.Debug("Effect pushed on stack: {0}.", effect);
    }

    public void BeginResolve()
    {
      LastResolved = null;

      if(TopSpell == null)
        return;

      var effect = TopSpell;
      Remove(effect);

      if (effect.CanBeResolved())
      {
        effect.BeginResolve();
      }

      LastResolved = effect;
    }

    public void FinishResolve()
    {
      var effect = Stack.LastResolved;
      if (effect != null)
      {
        effect.FinishResolve();
      }
    }

    public override string ToString()
    {
      return String.Format("{0}", Count);
    }

    public void GenerateTargets(Func<Zone, Player, bool> zoneFilter, List<ITarget> targets)
    {
      if (zoneFilter(Name, null))
      {
        targets.AddRange(_effects);
      }
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

      if (!TopSpell.HasEffectTargets() && targetOnly)
        return 0;

      var total = TopSpell.CalculateCreatureDamage(card);
      var prevented = card.CalculatePreventedDamageAmount(total, TopSpell.Source.OwningCard);

      return total - prevented;
    }

    public bool CanBeDealtLeathalDamageByTopSpell(Card card, bool targetOnly = false)
    {
      if (!card.Is().Creature)
        return false;

      var dealtAmount = GetDamageTopSpellWillDealToCreature(card, targetOnly);
      return card.Life <= dealtAmount;
    }

    public bool IsTargetedByTopSpell(Card card)
    {
      if (IsEmpty)
        return false;

      return TopSpell.HasEffectTarget(card);
    }

    public bool CanBeDestroyedByTopSpell(Card card, bool targetOnly = false)
    {
      if (IsEmpty)
        return false;

      if (card.CanBeDestroyed == false)
        return false;

      if (TopSpell.HasTag(EffectTag.Destroy))
      {
        return TopSpell.HasEffectTargets()
          ? TopSpell.HasEffectTarget(card)
          : !targetOnly;
      }

      if (card.Is().Creature == false)
        return false;

      return CanThougnessBeReducedToLeathalByTopSpell(card) || CanBeDealtLeathalDamageByTopSpell(card);
    }

    public bool CanTopSpellReducePlayersLifeToZero(Player player)
    {
      if (TopSpell == null)
        return false;

      var damage = TopSpell.CalculatePlayerDamage(player);
      return damage >= player.Life;
    }

    public bool TopSpellHas(EffectTag tag)
    {
      return !IsEmpty && TopSpell.HasTag(tag);
    }

    public bool HasSpellWithSource(Card card)
    {
      return _effects.Any(x => x.Source.OwningCard == card);
    }

    public bool HasSpellWithName(string name)
    {
      return _effects.Any(x => x.Source.OwningCard.Name.Equals(name));
    }

    public event EventHandler<StackChangedEventArgs> EffectAdded = delegate { };
    public event EventHandler<StackChangedEventArgs> EffectRemoved = delegate { };

    private void Remove(Effect effect)
    {
      _effects.Remove(effect);
      EffectRemoved(this, new StackChangedEventArgs(effect));

      LogFile.Debug("Effect removed from stack: {0}. (count: {1})", effect, _effects.Count);
    }

    private bool CanThougnessBeReducedToLeathalByTopSpell(Card card)
    {
      if (IsEmpty) return false;
      return card.Life <= TopSpell.CalculateToughnessReduction(card);
    }
  }
}