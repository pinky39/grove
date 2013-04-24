﻿namespace Grove.Gameplay.Card.Abilities
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;
  using Mana;
  using Modifiers;

  [Copyable]
  public class ActivatedAbilities : IModifiable, IHashable
  {
    private readonly TrackableList<ActivatedAbility> _abilities = new TrackableList<ActivatedAbility>();        

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(Card card, Game game)
    {            
      _abilities.Initialize(game.ChangeTracker, card);      

      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.Initialize(card, game);
      }    
    }

    public IEnumerable<ManaAbility> GetManaAbilities()
    {
      return _abilities.Where(x => x is ManaAbility).Select(x => (ManaAbility) x);
    }

    public void Activate(int abilityIndex, ActivationParameters activationParameters)
    {
      _abilities[abilityIndex].Activate(activationParameters);
    }

    public List<ActivationPrerequisites> CanActivate(bool ignoreManaAbilities)
    {
      var result = new List<ActivationPrerequisites>();

      for (var index = 0; index < _abilities.Count; index++)
      {
        var ability = _abilities[index];

        if (ignoreManaAbilities && ability is ManaAbility)
        {
          continue;
        }

        ActivationPrerequisites prerequisites;
        if (ability.CanActivate(out prerequisites))
        {
          prerequisites.Index = index;
          result.Add(prerequisites);
        }
      }

      return result;
    }

    public IManaAmount GetManaCost(int index)
    {
      return _abilities[index].GetManaCost();
    }

    public void EnableAll()
    {
      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.IsEnabled = true;
      }
    }

    public void DisableAll()
    {
      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.IsEnabled = false;
      }
    }

    public void Add(ActivatedAbility ability)
    {
      _abilities.Add(ability);
      ability.OnAbilityAdded();
    }  

    public void Remove(ActivatedAbility ability)
    {
      _abilities.Remove(ability);      
      ability.OnAbilityRemoved();
    }    

    public ActivatedAbility RemoveFirst()
    {
      var ability = _abilities.First();
      Remove(ability);
      return ability;
    }
  }
}