namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsReturnToHand : Decisions.SelectCardsReturnToHand
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: false);
    }
  }
}