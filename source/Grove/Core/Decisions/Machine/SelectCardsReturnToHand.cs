namespace Grove.Core.Controllers.Machine
{
  public class SelectCardsReturnToHand : Controllers.SelectCardsReturnToHand
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: false);
    }
  }
}