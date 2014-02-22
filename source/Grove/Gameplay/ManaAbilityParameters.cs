namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class ManaAbilityParameters : ActivatedAbilityParameters
  {
    public int CostRestriction;
    public int Priority = ManaSourcePriorities.Land;
    public bool SacRestriction;
    public bool TapRestriction;
    public ManaUsage UsageRestriction = ManaUsage.Any;

    public ManaAbilityParameters()
    {
      UsesStack = false;
    }

    public List<int> Colors { get; private set; }
    public ManaOutput ManaOutput { get; private set; }

    public void ManaAmount(IManaAmount amount)
    {
      ManaOutput = new FixedManaOutput(amount);
      Colors = amount.Colors.ToList();
    }

    public void ManaAmount(ManaColor color, Func<Card, bool> filter, ControlledBy controlledBy = ControlledBy.SpellOwner)
    {
      ManaOutput = new PermanentCountManaOutput(color, filter, controlledBy);
      Colors = color.Indices;
    }
  }
}