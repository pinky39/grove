namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsPutToBattlefield : Decisions.SelectCardsPutToBattlefield
  {
    protected override void ExecuteQuery()
    {
      Result = GetTargets(descending: true);      
    }
  }
}