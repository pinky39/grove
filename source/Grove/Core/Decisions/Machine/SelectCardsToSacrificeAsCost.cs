namespace Grove.Core.Controllers.Machine
{
  public class SelectCardsToSacrificeAsCost : Controllers.SelectCardsToSacrificeAsCost
  {
    protected override void ExecuteQuery()
    {
      if (Ai(Controller, CardToPayUpkeepFor))
      {
        CardSelector.ExecuteQueury(this, descending: false);  
      }            
    }
  }
}