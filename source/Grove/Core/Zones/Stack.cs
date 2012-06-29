namespace Grove.Core.Zones
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Effects;
  using Infrastructure;

  [Copyable]
  public class Stack : IEnumerable<Effect>, IHashable
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

      if (effect.HasEffectStillValidTargets())
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

    public bool CanBeDestroyedByTopSpell(Card card)
    {
      if (IsEmpty)
        return false;

      if (!card.CanBeDestroyed)
        return false;

      if (TopSpell.HasCategory(EffectCategories.Destruction))
      {
        if (!TopSpell.HasTargets)
          return true;

        return TopSpell.HasTarget(card);
      }

      var damageDealing = TopSpell as IDamageDealing;

      if (damageDealing == null)
        return false;

      var damage = new Damage(TopSpell.Source.OwningCard, damageDealing.CreatureDamage(card));
      var dealtAmount = card.CalculateDealtDamageAmount(damage);

      return damage.IsLeathal || card.LifepointsLeft <= dealtAmount;
    }
  }
}