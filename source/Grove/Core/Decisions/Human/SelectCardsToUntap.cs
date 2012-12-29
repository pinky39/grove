namespace Grove.Core.Decisions.Human
{
  public class SelectCardsToUntap : Decisions.SelectCardsToUntap
  {
    public CardSelector CardSelector { get; set; }
    
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}