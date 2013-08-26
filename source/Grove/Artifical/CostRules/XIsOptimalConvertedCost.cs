namespace Grove.Artifical.CostRules
{
  using System;
  using System.Linq;
  using Gameplay;

  public class XIsOptimalConvertedCost : CostRule
  {
    private readonly Func<Card, bool> _selector;

    private XIsOptimalConvertedCost() {}

    public XIsOptimalConvertedCost(Func<Card, bool> selector)
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
          .Where(x => _selector(x))
          .Where(x => x.ConvertedCost <= i)          
          .Sum(x => x.Score);

        var opponentsScore = p.Controller.Opponent.Battlefield
          .Where(x => _selector(x))
          .Where(x => x.ConvertedCost <= i)          
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