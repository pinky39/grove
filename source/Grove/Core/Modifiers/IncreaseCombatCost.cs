namespace Grove.Modifiers
{
  public class IncreaseCombatCost : Modifier, ICardModifier
  {
    private readonly int _value;
    private CombatCost _combatCost;
    private IntegerIncrement _increment;

    private IncreaseCombatCost() {}

    public IncreaseCombatCost(int value)
    {
      _value = value;
    }

    public override void Apply(CombatCost combatCost)
    {
      _combatCost = combatCost;
      _increment = new IntegerIncrement(_value);
      _increment.Initialize(ChangeTracker);
      _combatCost.AddModifier(_increment);
    }

    protected override void Unapply()
    {
      _combatCost.RemoveModifier(_increment);
    }
  }
}