namespace Grove.Core.Decisions.Human
{
  public class SelectCards : Decisions.SelectCards
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}