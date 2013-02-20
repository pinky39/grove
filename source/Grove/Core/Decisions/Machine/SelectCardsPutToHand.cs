namespace Grove.Core.Decisions.Machine
{
  public class SelectCardsPutToHand : Decisions.SelectCardsPutToHand
  {
    protected override void ExecuteQuery()
    {
      Result = GetTargets(descending: AiOrdersByDescendingScore);           
    }
  }
}