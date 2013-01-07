namespace Grove.Core.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Infrastructure;
  using Mana;
  using Modifiers;
  using Targeting;

  [Copyable]
  public class ActivatedAbilities : IModifiable, IHashable
  {
    private readonly TrackableList<ActivatedAbility> _abilities;
    private readonly TrackableList<IManaSource> _manaSources;

    private ActivatedAbilities() {}

    public ActivatedAbilities(IEnumerable<ActivatedAbility> abilities, ChangeTracker changeTracker,
      IHashDependancy hashDependancy)
    {
      _abilities = new TrackableList<ActivatedAbility>(abilities, changeTracker, hashDependancy);

      var manaSources = abilities
        .Where(x => x is IManaSource)
        .Cast<IManaSource>();

      _manaSources = new TrackableList<IManaSource>(manaSources, changeTracker);
    }

    public IList<IManaSource> ManaSources { get { return _manaSources; } }
    public IEnumerable<ManaAbility> ManaAbilities { get { return _abilities.Where(x => x is ManaAbility).Cast<ManaAbility>(); } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Activate(int abilityIndex, ActivationParameters activationParameters)
    {
      _abilities[abilityIndex].Activate(activationParameters);
    }

    public List<SpellPrerequisites> CanActivate(bool ignoreManaAbilities)
    {
      var result = new List<SpellPrerequisites>();

      for (int i = 0; i < _abilities.Count; i++)
      {
        var ability = _abilities[i];

        if (ignoreManaAbilities && ability is ManaAbility)
        {
          result.Add(SpellPrerequisites.CannotBeSatisfied);
          continue;
        }

        result.Add(ability.CanActivate());
      }

      return result;
    }

    public SpellPrerequisites CanActivate(int abilityIndex)
    {
      return _abilities[abilityIndex].CanActivate();
    }

    public void EquipTarget(Card target)
    {
       var effect = CreateEffect<Attach>(target);      
        effect.Resolve();
    }

    private T CreateEffect<T>(ITarget target) where T : Effect
    {
      foreach (var ability in _abilities)
      {
        var effect = ability.CreateEffect<T>(target);

        if (effect != null)
          return effect;
      }

      return null;
    }

    public IManaAmount GetManaCost(int index)
    {
      if (_abilities.Count <= index)
        throw new InvalidOperationException(String.Format("No ability with index {0} exists.", index));

      return _abilities[index].ManaCost;
    }

    public void Enable()
    {
      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.Enable();
      }
    }

    public void Disable()
    {
      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.Disable();
      }
    }

    public void Add(ActivatedAbility ability)
    {
      _abilities.Add(ability);
      AddManaSource(ability);
    }

    private void AddManaSource(ActivatedAbility ability)
    {
      var manaSource = ability as IManaSource;
      if (manaSource == null)
        return;

      _manaSources.Add(manaSource);
      ability.Controller.AddManaSource(manaSource);
    }

    public void Remove(ActivatedAbility ability)
    {
      _abilities.Remove(ability);

      RemoveManaSource(ability);
    }

    private void RemoveManaSource(ActivatedAbility ability)
    {
      var manaSource = ability as IManaSource;
      if (manaSource == null)
        return;

      _manaSources.Remove(manaSource);
      ability.Controller.RemoveManaSource(manaSource);
    }
  }
}