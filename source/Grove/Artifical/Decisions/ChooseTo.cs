namespace Grove.Artifical.Decisions
{
  public class ChooseTo : Gameplay.Decisions.ChooseTo
  {
    public ChooseTo()
    {
      Result = false;
    }
    
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult();
    }
  }
}