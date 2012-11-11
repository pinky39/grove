namespace Grove.Core.Controllers.Machine
{
  public class SelectCardsPutToBattlefield : Controllers.SelectCardsPutToBattlefield
  {
    protected override void ExecuteQuery()
    {
      CardSelector.ExecuteQueury(this, descending: true);
    }
  }
}