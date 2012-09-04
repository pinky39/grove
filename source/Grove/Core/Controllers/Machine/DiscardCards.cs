namespace Grove.Core.Controllers.Machine
{
  using System.Linq;
  using Ai;

  public class DiscardCards : Controllers.DiscardCards
  {
    protected override void ExecuteQuery()
    {            
      var cardsToDiscard = CardsOwner.Hand.Select(
        card =>
          new
            {
              Card = card,
              Score = ScoreCalculator.CalculateDiscardScore(card)
            });

      cardsToDiscard = DiscardOpponentsCards 
        ? cardsToDiscard.OrderByDescending(x => x.Score) 
        : cardsToDiscard.OrderBy(x => x.Score);
        
      Result = cardsToDiscard
        .Take(Count)
        .Select(x => x.Card)
        .ToList();
    }
  }
}