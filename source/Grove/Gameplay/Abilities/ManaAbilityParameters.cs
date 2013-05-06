namespace Grove.Gameplay.Abilities
{
  using System;
  using ManaHandling;
  using Misc;

  public class ManaAbilityParameters : ActivatedAbilityParameters
  {
    public int CostRestriction;
    public int Priority = ManaSourcePriorities.Land;
    public bool TapRestriction;
    public ManaUsage UsageRestriction = ManaUsage.Any;

    public ManaAbilityParameters()
    {
      UsesStack = false;
    }

    public ManaOutput ManaOutput { get; private set; }

    public void ManaAmount(IManaAmount amount)
    {
      ManaOutput = new FixedManaOutput(amount);
    }

    public void ManaAmount(ManaColor color, Func<Card, bool> filter, ControlledBy controlledBy = ControlledBy.SpellOwner)
    {
      ManaOutput = new PermanentCountManaOutput(color, filter, controlledBy);
    }
  }
}