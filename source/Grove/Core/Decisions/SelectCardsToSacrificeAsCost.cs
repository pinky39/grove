namespace Grove.Core.Controllers
{
  using System;

  public abstract class SelectCardsToSacrificeAsCost : SelectCards
  {
    public Func<Player, Card, bool> Ai = delegate { return true; };    
    public string QuestionText { get; set; }
    public Card CardToPayUpkeepFor { get; set; }

    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.Sacrifice();
    }
  }
}