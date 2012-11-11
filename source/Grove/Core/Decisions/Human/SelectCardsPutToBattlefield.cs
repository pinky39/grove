namespace Grove.Core.Controllers.Human
{
  public class SelectCardsPutToBattlefield : Controllers.SelectCardsPutToBattlefield
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}