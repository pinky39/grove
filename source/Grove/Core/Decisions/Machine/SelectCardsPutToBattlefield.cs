namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsPutToBattlefield : Decisions.SelectCardsPutToBattlefield
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: true);
    }
  }
}