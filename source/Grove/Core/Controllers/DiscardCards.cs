namespace Grove.Core.Controllers
{
  using System;
  using System.Linq;
  using Results;

  public abstract class DiscardCards : Decision<ChosenCards>
  {
    public int Count { get; set; }
    public bool DiscardOpponentsCards { get; set; }
    public Func<Card, bool> Filter = delegate { return true; };

    protected Player CardsOwner
    {
      get { return DiscardOpponentsCards 
        ? Game.Players.GetOpponent(Controller) 
        : Controller; }
    }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        return Count > 0 && 
          CardsOwner.Hand.Where(Filter).Count() > Count;
      }
    }

    public override void ProcessResults()
    {
      if (Count == 0)
        return;

      var candidates = CardsOwner.Hand.Where(Filter).ToList();
      
      if (candidates.Count <= Count)
      {
        foreach (var card in candidates)
        {
          card.Discard();
        }                
        return;
      }

      foreach (var card in Result)
      {
        card.Discard();
      }
    }
  }
}