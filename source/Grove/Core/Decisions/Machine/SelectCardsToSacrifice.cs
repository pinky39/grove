namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsToSacrifice : Decisions.SelectCardsToSacrifice
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: false);
    }
  }
}