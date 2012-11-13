namespace Grove.Core.Decisions.Human
{
  public class SelectCardsToSacrifice : Decisions.SelectCardsToSacrifice
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}