namespace Grove.Gameplay.Decisions
{
  using System.Collections.Generic;
  using Effects;
  using Results;

  public abstract class ChooseEffectOptions : Decision<ChosenOptions>
  {
    public List<IEffectChoice> Choices;
    public IChooseDecisionResults<List<IEffectChoice>, ChosenOptions> ChooseDecisionResults;
    public IProcessDecisionResults<ChosenOptions> ProcessDecisionResults;
    public string Text;    

    protected override bool ShouldExecuteQuery { get { return true; } }
    

    public override void ProcessResults()
    {
      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}