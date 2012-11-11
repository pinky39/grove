namespace Grove.Core.Controllers.Human
{
  public class SelectCardsToSacrifice : Controllers.SelectCardsToSacrifice
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}