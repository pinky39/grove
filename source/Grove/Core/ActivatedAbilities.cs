namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Mana;
  using Modifiers;

  [Copyable]
  public class ActivatedAbilities : IModifiable, IHashable
  {
    private readonly TrackableList<ActivatedAbility> _abilities = new TrackableList<ActivatedAbility>();

    public IEnumerable<IManaSource> ManaSources { get { return _abilities.Where(x => x is IManaSource).Cast<IManaSource>(); } }
    public IEnumerable<ManaAbility> ManaAbilities { get { return _abilities.Where(x => x is ManaAbility).Cast<ManaAbility>(); } }

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

      foreach (var ability in _abilities)
      {                
        AddManaSource(ability);
      }
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
      AddManaSource(ability);
    }

    private void AddManaSource(ActivatedAbility ability)
    {
      var manaSource = ability as IManaSource;
      if (manaSource == null)
        return;      

      if (ability.IsInitialized)
        ability.OwningCard.Controller.AddManaSource(manaSource);
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
      
      ability.OwningCard.Controller.RemoveManaSource(manaSource);
    }

    public ActivatedAbility RemoveFirst()
    {
      var ability = _abilities.First();
      _abilities.Remove(ability);
      RemoveManaSource(ability);

      return ability;
    }
  }
}