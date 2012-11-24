namespace Grove.Core.Decisions
{
  public abstract class SelectCardsPutToHand : SelectCards
  {
    public bool ShouldAlwaysExecuteQuery;
    public bool AiOrdersByDescendingScore;
    
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.PutToHand();
    }

    protected override bool ShouldExecuteQuery
    {
      get { return ShouldAlwaysExecuteQuery || base.ShouldExecuteQuery; }
    }
  }
}