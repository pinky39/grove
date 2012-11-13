namespace Grove.Core.Decisions.Machine
{
  using System.Linq;
  using Grove.Core.Ai;

  public class DiscardCards : Decisions.DiscardCards
  {
    protected override void ExecuteQuery()
    {            
      var cardsToDiscard = CardsOwner.Hand
      .Where(Filter)
      .Select(
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