namespace Grove.Core.Details.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Infrastructure;
  using Mana;
  using Modifiers;

  
  [Copyable]
  public class ActivatedAbilities : IModifiable, IHashable
  {
    private readonly TrackableList<ActivatedAbility> _abilities;


    private ActivatedAbilities() {}

    public ActivatedAbilities(IEnumerable<ActivatedAbility> abilities, ChangeTracker changeTracker,
                              IHashDependancy hashDependancy)
    {
      _abilities = new TrackableList<ActivatedAbility>(abilities, changeTracker, hashDependancy);
    }

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

    public List<SpellPrerequisites> CanActivate()
    {
      return _abilities.Select(ability => ability.CanActivate()).ToList();
    }

    public SpellPrerequisites CanActivate(int abilityIndex)
    {
      return _abilities[abilityIndex].CanActivate();
    }

    public T GetEffect<T>() where T : Effect
    {
      foreach (var ability in _abilities)
      {
        var effect = ability.GetEffect<T>();

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
  }
}