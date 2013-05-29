namespace Grove.Artifical.Decisions
{
  using System.Linq;

  public class DiscardCards : Gameplay.Decisions.DiscardCards
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