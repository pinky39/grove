namespace Grove.Artifical.Decisions
{
  using System.Linq;
  using Gameplay.Decisions.Results;

  public class DiscardCards : Gameplay.Decisions.DiscardCards
  {
    public DiscardCards()
    {
      Result = new ChosenCards();
    }
    
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