namespace Grove.Core.Ai
{
  using System;

  public delegate int CalculateX(XCalculatorParameters parameters);

  public static class VariableCost
  {
    public static CalculateX ReduceCreaturesPwT()
    {
      return p =>
        {
          const int maxXToTry = 6;

          var yourCreatures = p.Controller.Battlefield.Creatures;
          var opponentCreatures = p.Opponent.Battlefield.Creatures;

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

          for (var i = 1; i < maxXToTry; i++)
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
      return p =>
        {
          var lifepoints = p.Target.LifepointsLeft();

          if (p.Target.IsPlayer())
          {
            const int minDamageToPlayer = 5;
            var maxX = p.Controller.ConvertedMana - p.Source.ManaCost.Converted;

            var max = Math.Max(minDamageToPlayer, maxX);
            return Math.Min(max, lifepoints);
          }

          return lifepoints;
        };
    }
  }
}