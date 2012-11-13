namespace Grove.Core.Decisions.Human
{
  public class SelectCardsPutToBattlefield : Decisions.SelectCardsPutToBattlefield
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}