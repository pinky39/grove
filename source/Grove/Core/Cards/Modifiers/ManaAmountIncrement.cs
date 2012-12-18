﻿namespace Grove.Core.Cards.Modifiers
{
  using Infrastructure;
  using Mana;

  public class ManaAmountIncrement : PropertyModifier<IManaAmount>
  {
    private readonly IManaAmount _increment;

    public ManaAmountIncrement(IManaAmount increment, ChangeTracker changeTracker) : base(changeTracker)
    {
      _increment = increment;
    }

    public override int Priority { get { return 1; } }

    public override IManaAmount Apply(IManaAmount before)
    {
      return new AggregateManaAmount(_increment, before);
    }
  }
}