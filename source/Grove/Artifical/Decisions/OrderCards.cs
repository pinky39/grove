namespace Grove.Artifical.Decisions
{
  using Gameplay.Decisions.Results;

  public class OrderCards : Gameplay.Decisions.OrderCards
  {
    public OrderCards()
    {
      Result = new Ordering();
    }
    
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(Cards);
    }
  }
}