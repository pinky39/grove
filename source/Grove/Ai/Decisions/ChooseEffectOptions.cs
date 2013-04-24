namespace Grove.Ai.Decisions
{
  public class ChooseEffectOptions : Gameplay.Decisions.ChooseEffectOptions
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(Choices);
    }
  }
}