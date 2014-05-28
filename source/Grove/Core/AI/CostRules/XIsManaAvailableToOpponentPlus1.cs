namespace Grove.AI.CostRules
{
  public class XIsManaAvailableToOpponentPlus1 : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      if (Stack.IsEmpty || Stack.TopSpellOwner == p.Controller)
        return int.MaxValue;

      return p.Controller.Opponent.GetAvailableConvertedMana() + 1;
    }
  }
}