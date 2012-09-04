namespace Grove.Core.Controllers
{
  using Results;

  public abstract class DiscardCards : Decision<ChosenCards>
  {
    public int  Count { get; set; }
    public bool DiscardOpponentsCards { get; set; }

    protected Player CardsOwner {get { return DiscardOpponentsCards ? Game.Players.GetOpponent(Controller) : Controller; }}

    protected override bool ShouldExecuteQuery { get { return Count > 0 && CardsOwner.Hand.Count > Count; } }

    public override void ProcessResults()
    {
      if (Count == 0)
        return;

      if (CardsOwner.Hand.Count <= Count)
      {
        CardsOwner.DiscardHand();
        return;
      }

      foreach (var card in Result)
      {                
        card.Discard();
      }
    }
  }
}