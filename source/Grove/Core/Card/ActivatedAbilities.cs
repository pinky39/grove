namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class ActivatedAbilities : IAcceptsCardModifier, IHashable
  {
    private readonly TrackableList<ActivatedAbility> _abilities;

    private ActivatedAbilities() {}

    public ActivatedAbilities(IEnumerable<ActivatedAbility> activatedAbilities)
    {
      _abilities = new TrackableList<ActivatedAbility>(activatedAbilities);
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
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
      return _abilities.OfType<ManaAbility>();
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

    public IEnumerable<IManaAmount> GetManaCost()
    {
      return _abilities.Select(x => x.GetManaCost());
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