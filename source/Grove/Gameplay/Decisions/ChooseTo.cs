namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class ChooseTo : Decision<BooleanResult>
  {
    public string Text;
    public IChooseDecisionResults<BooleanResult> ChooseDecisionResults;
    public IProcessDecisionResults<BooleanResult> ProcessDecisionResults;
        
    public override void ProcessResults()
    {
      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}