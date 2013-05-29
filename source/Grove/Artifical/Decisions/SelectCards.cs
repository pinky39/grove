namespace Grove.Artifical.Decisions
{
  public class SelectCards : Gameplay.Decisions.SelectCards
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(ValidTargets);
    }
  }
}