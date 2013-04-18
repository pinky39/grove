namespace Grove.Core.Mana
{
  using System;

  public class ManaAbilityParameters : ActivatedAbilityParameters
  {
    public int Priority = ManaSourcePriorities.Land;
    public bool TapRestriction;
    public int CostRestriction;
    public ManaOutput ManaOutput { get; private set; }
    public ManaUsage UsageRestriction = ManaUsage.Any;

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