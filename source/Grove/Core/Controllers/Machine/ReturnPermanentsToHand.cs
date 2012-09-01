namespace Grove.Core.Controllers.Machine
{
  using System.Linq;
  using Ai;

  public class ReturnPermanentsToHand : Controllers.ReturnPermanentsToHand
  {
    protected override void ExecuteQuery()
    {
      var permenents = Controller.Battlefield
        .Where(Filter)
        .Select(card => new
          {
            Card = card,
            Score = ScoreCalculator.CalculatePermanentScore(card)
          })
        .OrderBy(x => x.Score)
        .Select(x => x.Card).Take(Count);

      Result = permenents.ToList();
    }
  }
}