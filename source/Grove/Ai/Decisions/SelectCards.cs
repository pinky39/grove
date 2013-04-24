namespace Grove.Core.Decisions.Machine
{
  public class SelectCards : Decisions.SelectCards
  {
    protected override void ExecuteQuery()
    {      
      Result = ChooseDecisionResults.ChooseResult(ValidTargets);
    }
  }
}