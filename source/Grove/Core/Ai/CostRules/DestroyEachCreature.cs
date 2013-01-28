namespace Grove.Core.Ai.CostRules
{
  using System;
  using System.Linq;
  using Mana;

  public class DestroyEachCreature : CostRule
  {
    public Func<Card, int, bool> Selector = delegate { return true; };
    public ManaUsage ManaUsage = ManaUsage.Spells;
    
    public override int CalculateX(CostRuleParameters p)
    {
      var max = p.GetMaxConvertedMana(ManaUsage);

      var bestX = max + 1;
      var bestScore = -1;

      for (var i = max; i >= 0; i--)
      {
        var yourScore = p.Controller.Battlefield
          .Where(x => Selector(x, i))
          .Sum(x => x.Score);
            
        var opponentsScore = p.Controller.Opponent.Battlefield
          .Where(x => Selector(x, i))
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