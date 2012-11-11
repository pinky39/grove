namespace Grove.Core.Controllers
{
  using System;

  public abstract class SelectCardsToSacrificeAsCost : SelectCards
  {
    public Func<Player, Card, bool> Ai = delegate { return true; };    
    public string QuestionText { get; set; }
    public Card CardToPayUpkeepFor { get; set; }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        var count = ValidCards.Count;
        return count >= MinCount;
      }
    }
    
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.Sacrifice();
    }
  }
}