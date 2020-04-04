namespace Grove.AI.CostRules
{
  public class XIsManaAvailableToOpponentPlus1 : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      if (Stack.IsEmpty || Stack.TopSpellOwner == p.Controller)
        return int.MaxValue;

      return p.Controller.Opponent.GetAvailableManaCount(        
        new ConvokeAndDelveOptions
        {
          CanUseConvoke = p.OwningCard.Has().Convoke,
          CanUseDelve = p.OwningCard.Has().Delve,
        }) + 1;
    }
  }
}