namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Linq;
  using Results;

  public abstract class DiscardCards : Decision<ChosenCards>
  {
    public Func<Card, bool> Filter = delegate { return true; };
    public int Count { get; set; }
    public bool DiscardOpponentsCards { get; set; }

    protected Player CardsOwner
    {
      get
      {
        return DiscardOpponentsCards
          ? Controller.Opponent
          : Controller;
      }
    }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        return Count > 0 &&
          CardsOwner.Hand.Where(Filter).Count() > Count;
      }
    }

    protected override void SetResultNoQuery()
    {
      Result = new ChosenCards(CardsOwner.Hand.Where(Filter));
    }

    public override void ProcessResults()
    {     
      foreach (var card in Result)
      {
        card.Discard();
      }
    }
  }
}