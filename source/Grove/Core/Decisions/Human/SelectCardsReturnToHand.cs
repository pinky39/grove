namespace Grove.Core.Controllers.Human
{
  public class SelectCardsReturnToHand : Controllers.SelectCardsReturnToHand
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}