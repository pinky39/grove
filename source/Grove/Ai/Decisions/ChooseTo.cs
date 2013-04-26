namespace Grove.Ai.Decisions
{
  public class ChooseTo : Gameplay.Decisions.ChooseTo
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult();
    }
  }
}