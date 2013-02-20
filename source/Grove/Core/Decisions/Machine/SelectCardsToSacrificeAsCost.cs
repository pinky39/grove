namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsToSacrificeAsCost : Decisions.SelectCardsToSacrificeAsCost
  {
    protected override void ExecuteQuery()
    {
      if (Ai(Controller, CardToPayUpkeepFor))
      {
        Result = GetTargets(descending: false);                
      }            
    }
  }
}