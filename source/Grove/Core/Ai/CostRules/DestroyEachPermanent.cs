namespace Grove.Core.Ai.CostRules
{
  using System;
  using System.Linq;

  public class DestroyEachPermanent : CostRule
  {
    private readonly Func<Card, int, bool> _selector;

    private DestroyEachPermanent() {}

    public DestroyEachPermanent(Func<Card, int, bool> selector)
    {
      _selector = selector;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      var bestX = p.MaxX + 1;
      var bestScore = -1;

      for (var i = p.MaxX; i >= 0; i--)
      {
        var yourScore = p.Controller.Battlefield
          .Where(x => _selector(x, i))
          .Sum(x => x.Score);

        var opponentsScore = p.Controller.Opponent.Battlefield
          .Where(x => _selector(x, i))
          .Sum(x => x.Score);

        var diff = opponentsScore - yourScore;

        if (diff >= bestScore)
        {
          bestScore = diff;
          bestX = i;
        }
      }

      return bestX;
    }
  }
}