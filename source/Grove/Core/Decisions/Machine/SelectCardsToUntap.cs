namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsToUntap : Decisions.SelectCardsToUntap
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: true);
    }
  }
}