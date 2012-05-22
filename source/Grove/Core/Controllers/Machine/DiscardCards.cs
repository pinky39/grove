namespace Grove.Core.Controllers.Machine
{
  using System.Linq;
  using Ai;

  public class DiscardCards : Controllers.DiscardCards
  {
    protected override void ExecuteQuery()
    {
      var cardsToDiscard = Player.Hand.Select(
        card =>
          new{
            Card = card,
            Score = ScoreCalculator.CalculateDiscardScore(card)
          })
        .OrderBy(x => x.Score)
        .Take(Count)
        .Select(x => x.Card);

      Result = cardsToDiscard.ToList();
    }
  }
}