namespace Grove.Core.Ai.CostRules
{
  using System;
  using System.Linq;
  using Mana;

  public class DestroyEachPermanent : CostRule
  {
    private readonly ManaUsage _manaUsage;
    private readonly Func<Card, int, bool> _selector;

    public DestroyEachPermanent(Func<Card, int, bool> selector, ManaUsage manaUsage)
    {
      _selector = selector;
      _manaUsage = manaUsage;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      var max = p.GetMaxConvertedMana(_manaUsage);

      var bestX = max + 1;
      var bestScore = -1;

      for (var i = max; i >= 0; i--)
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