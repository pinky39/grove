namespace Grove.Artifical.Decisions
{
  using Gameplay.Decisions.Results;

  public class ChooseEffectOptions : Gameplay.Decisions.ChooseEffectOptions
  {
    public ChooseEffectOptions()
    {
      Result = new ChosenOptions();
    }
    
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(Choices);
    }
  }
}