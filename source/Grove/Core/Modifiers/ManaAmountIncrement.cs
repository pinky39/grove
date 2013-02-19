namespace Grove.Core.Modifiers
{
  using Mana;

  public class ManaAmountIncrement : PropertyModifier<IManaAmount>
  {
    private readonly IManaAmount _increment;

    public ManaAmountIncrement(IManaAmount increment)
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