namespace Grove.Artifical.Decisions
{
  public class PayOr : Gameplay.Decisions.PayOr
  {
    public PayOr()
    {
      Result = false;
    }
    
    protected override void ExecuteQuery()
    {
      if (ChooseDecisionResults != null)
      {
        Result = ChooseDecisionResults.ChooseResult();
        return;
      }

      Result = true;
    }
  }
}