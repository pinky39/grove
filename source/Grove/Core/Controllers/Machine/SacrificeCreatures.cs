namespace Grove.Core.Controllers.Machine
{
  using System.Linq;
  using Ai;

  public class SacrificeCreatures : Controllers.SacrificeCreatures
  {
    protected override void ExecuteQuery()
    {
      var creaturesToSacrifice = Player.Battlefield
        .Creatures
        .Select(card => new
          {
            Card = card,
            Score = ScoreCalculator.CalculatePermanentScore(card)
          })
        .OrderBy(x => x.Score)
        .Select(x => x.Card).Take(Count);

      Result = creaturesToSacrifice.ToList();
    }
  }
}