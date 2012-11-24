namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsPutToHand : Decisions.SelectCardsPutToHand
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: AiOrdersByDescendingScore);
    }
  }
}