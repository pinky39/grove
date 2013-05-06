namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class ChooseTo : Decision<BooleanResult>
  {
    public IChooseDecisionResults<BooleanResult> ChooseDecisionResults;
    public IProcessDecisionResults<BooleanResult> ProcessDecisionResults;
    public string Text;

    public override void ProcessResults()
    {
      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}