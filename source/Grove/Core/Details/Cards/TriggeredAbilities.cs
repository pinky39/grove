namespace Grove.Core.Details.Cards
{
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class TriggeredAbilities : IModifiable, IHashable
  {
    private readonly TrackableList<TriggeredAbility> _abilities;

    private TriggeredAbilities() {}

    public TriggeredAbilities(IEnumerable<TriggeredAbility> abilities, ChangeTracker changeTracker,
                              IHashDependancy hashDependancy)
    {
      _abilities = new TrackableList<TriggeredAbility>(abilities, changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(TriggeredAbility ability)
    {
      _abilities.Add(ability);
    }

    public void Remove(TriggeredAbility ability)
    {
      _abilities.Remove(ability);
      ability.Dispose();
    }

    public void Disable()
    {
      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.Disable();
      }
    }

    public void Enable()
    {
      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.Enable();
      }
    }
  }
}