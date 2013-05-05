namespace Grove.Ai.Decisions
{
  public class OrderCards : Gameplay.Decisions.OrderCards
  {
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult();
    }
  }
}