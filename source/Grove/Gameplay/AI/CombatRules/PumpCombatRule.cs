namespace Grove.Gameplay.AI.CombatRules
{
  public class PumpCombatRule : CombatRule
  {
    private readonly IManaAmount _cost;
    private readonly int _power;
    private readonly int _toughness;

    private PumpCombatRule() {}

    public PumpCombatRule(int power, int toughness, IManaAmount cost)
    {
      _power = power;
      _toughness = toughness;
      _cost = cost;
    }


    public override void Apply(CombatAbilities combatAbilities)
    {
      var cost = _cost;

      while (OwningCard.Controller.HasMana(cost, ManaUsage.Abilities))
      {
        combatAbilities.PowerIncrease += _power;
        combatAbilities.ToughnessIncrease += _toughness;

        cost = cost.Add(_cost);
      }
    }
  }
}