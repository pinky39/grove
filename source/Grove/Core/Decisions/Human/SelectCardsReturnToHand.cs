namespace Grove.Core.Decisions.Human
{
  public class SelectCardsReturnToHand : Decisions.SelectCardsReturnToHand
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}