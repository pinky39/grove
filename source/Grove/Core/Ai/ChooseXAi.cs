namespace Grove.Core.Ai
{
  using System;
  using System.Linq;
  using Mana;
  using Targeting;

  public delegate int CalculateX(XCalculatorParameters parameters);

  public static class ChooseXAi
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
            if (creature.Life <= maxXToTry)
              score[creature.Life - 1]++;
          }

          foreach (var creature in yourCreatures)
          {
            if (creature.Life <= maxXToTry)
              score[creature.Life - 1]--;
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

    public static CalculateX TargetLifepointsLeft(ManaUsage usage)
    {
      return p =>
        {
          var lifepoints = p.Target.Life();

          if (p.Target.IsPlayer())
          {
            const int minDamageToPlayer = 5;
            var maxX = p.Controller.GetConvertedMana(usage) - p.Source.ManaCost.Converted;

            var max = Math.Max(minDamageToPlayer, maxX);
            return Math.Min(max, lifepoints);
          }

          return lifepoints;
        };
    }

    public static CalculateX CounterUnlessPay()
    {
      return p =>
        {
          if (p.Game.Stack.IsEmpty || p.Game.Stack.TopSpellOwner == p.Controller)
            return int.MaxValue;

          return p.Opponent.GetConvertedMana(ManaUsage.Any) + 1;
        };
    }

    public static CalculateX ChangeToXXCreature()
    {
      return p => p.Controller.GetConvertedMana(ManaUsage.Abilities);
    }

    public static CalculateX MaximumAvailableMana(ManaUsage usage = ManaUsage.Spells)
    {
      return p => p.Controller.GetConvertedMana(usage);
    }

    public static CalculateX AtLeast(int amount)
    {
      return p =>
        {
          var max = p.Controller.GetConvertedMana(ManaUsage.Spells) - p.Source.ManaCost.Converted;
          return max > amount ? max : amount;
        };
    }

    public static CalculateX DestroyEach(Func<Card, int, bool> filter)
    {
      return p =>
        {
          var max = p.Controller.GetConvertedMana(ManaUsage.Spells) - p.Source.ManaCost.Converted;

          var bestX = max + 1;
          var bestScore = -1;

          for (var i = max; i >= 0; i--)
          {
            var yourScore = p.Controller.Battlefield
              .Where(x => filter(x, i))
              .Sum(x => x.Score);
            
            var opponentsScore = p.Opponent.Battlefield
              .Where(x => filter(x, i))
              .Sum(x => x.Score);
            
            var diff = opponentsScore - yourScore;

            if (diff >= bestScore)
            {
              bestScore = diff;
              bestX = i;
            }
          }

          return bestX;
        };
    }
  }
}