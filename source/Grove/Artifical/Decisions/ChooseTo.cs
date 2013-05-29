namespace Grove.Artifical.Decisions
{
  public class ChooseTo : Gameplay.Decisions.ChooseTo
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult();
    }
  }
}