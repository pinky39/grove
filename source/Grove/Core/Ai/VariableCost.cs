namespace Grove.Core.Ai
{
  using System;

  public delegate int CalculateX(Players players, Card source, ITarget target);

  public static class VariableCost
  {
    public static CalculateX ReduceCreaturesPwT()
    {
      return (players, source, _) => {
        const int maxXToTry = 6;

        var yourCreatures = source.Controller.Battlefield.Creatures;
        var opponentCreatures = players.GetOpponent(source.Controller).Battlefield.Creatures;

        var score = new int[maxXToTry];

        foreach (var creature in opponentCreatures)
        {
          if (creature.LifepointsLeft <= maxXToTry)            
            score[creature.LifepointsLeft - 1]++;
        }

        foreach (var creature in yourCreatures)
        {
          if (creature.LifepointsLeft <= maxXToTry)
            score[creature.LifepointsLeft - 1]--;
        }
        
        for (int i = 1; i < maxXToTry; i++)
        {          
          score[i] += score[i - 1];
        }
        
        var result = int.MaxValue;
        var best = 0;

        for (var i = 0; i < maxXToTry; i++)
        {
          if (score[i] > best)
          {
            best = score[i];
            result = i + 1;
          }
        }

        return result;
      };
    }

    public static CalculateX TargetLifepointsLeft()
    {
      return (players, source, target) => {

        var lifepoints = target.LifepointsLeft();
        
        if (target.IsPlayer())
        {
          const int minDamageToPlayer = 5;        
          var maxX = source.Controller.ConvertedMana - source.ManaCost.Converted;

          var max = Math.Max(minDamageToPlayer, maxX);
          return Math.Min(max, lifepoints);
        }

        return lifepoints;
      };
    }
  }
}