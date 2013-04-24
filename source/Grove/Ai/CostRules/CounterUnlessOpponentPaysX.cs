namespace Grove.Ai.CostRules
{
  public class CounterUnlessOpponentPaysX : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      if (Stack.IsEmpty || Stack.TopSpellOwner == p.Controller)
        return int.MaxValue;

      return p.Controller.Opponent.GetConvertedMana() + 1;
    }
  }
}