namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Costs;

  public class ManaAbilityParameters : ActivatedAbilityParameters
  {
    public int Priority = ManaSourcePriorities.Land;
    public ManaUsage UsageRestriction = ManaUsage.Any;

    public ManaAbilityParameters()
    {
      Cost = new TapOwner();
      UsesStack = false;
    }

    public List<int> Colors { get; private set; }
    public ManaOutput ManaOutput { get; private set; }
    public readonly List<Effect.Factory> AdditionalEffects = new List<Effect.Factory>();

    public void ManaAmount(ManaAmount amount)
    {
      ManaOutput = new FixedManaOutput(amount);
      Colors = amount.Colors.ToList();
    }

    public void ManaAmount(ManaColor color, Func<Card, bool> filter,
      ControlledBy controlledBy = ControlledBy.SpellOwner)
    {
      ManaOutput = new PermanentCountManaOutput(color, filter, controlledBy);
      Colors = color.Indices;
    }
  }
}