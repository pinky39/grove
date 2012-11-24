namespace Grove.Core.Decisions.Human
{
  public class SelectCardsPutToHand : Decisions.SelectCardsPutToHand
  {
    public CardSelector CardSelector { get; set; }

    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQuery(this);
    }
  }
}