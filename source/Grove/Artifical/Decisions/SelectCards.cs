namespace Grove.Artifical.Decisions
{
  using Gameplay.Decisions.Results;

  public class SelectCards : Gameplay.Decisions.SelectCards
  {
    public SelectCards()
    {
      Result = new ChosenCards();
    }
    
    protected override void ExecuteQuery()
    {
      Result = ChooseDecisionResults.ChooseResult(ValidTargets);
    }
  }
}