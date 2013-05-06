namespace Grove.Artifical.Decisions
{
  public class OrderCards : Gameplay.Decisions.OrderCards
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult();
    }
  }
}