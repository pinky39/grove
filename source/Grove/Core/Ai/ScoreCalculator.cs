namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Mana;

  public static class ScoreCalculator
  {
    private static readonly Dictionary<int, int> LifeToScore = new Dictionary<int, int>
      {
        {20, 0},
        {19, -40},
        {18, -80},
        {17, -120},
        {16, -160},
        {15, -200},
        {14, -250},
        {13, -300},
        {12, -350},
        {11, -420},
        {10, -490},
        {9, -590},
        {8, -690},
        {7, -790},
        {6, -850},
        {5, -1000},
        {4, -1150},
        {3, -1400},
        {2, -1750},
        {1, -2300},
      };

    private static readonly Dictionary<int, int> ManaCostToScore = new Dictionary<int, int>
      {
        {0, 140},
        {1, 150},
        {2, 200},
        {3, 250},
        {4, 300},
        {5, 350},
        {6, 400},
        {7, 450},
      };

    private static readonly Dictionary<int, int> PowerToughnessToScore = new Dictionary<int, int>
      {
        {0, 0},
        {1, 140},
        {2, 160},
        {3, 180},
        {4, 200},
        {5, 220},
        {6, 240},
        {7, 260},
        {8, 280},
        {9, 300},
        {10, 320},
      };

    public static int CalculateDiscardScore(Card card)
    {
      // the lower the score, the more likely card will be discarded      
      var hand = card.Controller.Hand;
      var battlefield = card.Controller.Battlefield;

      var sameCardBattleFieldCount = battlefield.Count(x => x.Name == card.Name);

      if (card.Is().Land)
      {
        var landHandCount = hand.Count(x => x.Is().Land);
        var landBattlefieldCount = battlefield.Count(x => x.Is().Land);

        if ((landHandCount + landBattlefieldCount) < 4)
          return int.MaxValue;

        if (sameCardBattleFieldCount < 2)
          return int.MaxValue;

        if (landHandCount < 2)
          return int.MaxValue;

        return 0 - 2*sameCardBattleFieldCount;
      }

      return 0 - card.ManaCost.Count();
    }

    public static int CalculateLifeScore(int life)
    {
      var score = 5000;

      if (life > 20)
        return score + (life - 20)*40;

      if (life <= 0)
        return 500 + life;

      return score + LifeToScore[life];
    }

    public static int CalculatePermanentScore(Card permanent)
    {
      const int landValue = 150;
      const int tappedPermanentValue = -1;
      var score = 0;

      if (permanent.OverrideScore.HasValue)
        return permanent.OverrideScore.Value;

      if (permanent.IsTapped)
        score += tappedPermanentValue;

      if (permanent.Level > 0)
        score += 10*permanent.Level.Value;

      if (permanent.ManaCost != null)
      {
        score += CalculatePermanentScoreFromManaCost(permanent.ManaCost);

        if (permanent.Is().Creature)
        {
          score += (permanent.Power.Value + permanent.Toughness.Value);
        }
      }
      else if (permanent.Is().Creature)
      {
        score += CalculatePermanentScoreFromPowerToughness(permanent.Power.Value, permanent.Toughness.Value);
      }
      else if (permanent.Is().Land)
      {
        score += landValue;

        if (!permanent.Is().BasicLand)
          score += 10;
      }

      return score;
    }

    private static int CalculatePermanentScoreFromManaCost(IManaAmount mana)
    {
      var converted = Math.Min(7, mana.Converted);
      return ManaCostToScore[converted];
    }

    private static int CalculateCardInHandScoreFromManaCost(IManaAmount mana)
    {
      var converted = Math.Min(7, mana.Converted);
      var score = ManaCostToScore[converted] - 100;
      return score > 120 ? score : 120;
    }

    private static int CalculatePermanentScoreFromPowerToughness(int power, int toughness)
    {
      var powerToughness = power + toughness;

      if (powerToughness < 0)
        powerToughness = 0;
      else if (powerToughness > 10)
        powerToughness = 10;

      return PowerToughnessToScore[powerToughness];
    }

    public static int CalculateCardInHandScore(Card card)
    {          
      if (!card.IsVisible)
      {
        return 220;
      }

      if (card.OverrideScore.HasValue && (card.Is().Sorcery || card.Is().Instant))
      {
        return card.OverrideScore.Value;
      }
      
      if (card.ManaCost == null || card.ManaCost.Converted == 0)
      {
        return 110;
      }

      return CalculateCardInHandScoreFromManaCost(card.ManaCost);
    }

    public static int CalculateCardInGraveyardScore(Card card)
    {      
      if (card.Is().BasicLand)
        return 1;

      if (card.Is().Land)
        return 2;

      return card.ManaCost.Converted;
    }

    public static int CalculateLifelossScore(int life, int loss)
    {
      return CalculateLifeScore(life) - CalculateLifeScore(life - loss);
    }

    public static int CalculateCardInLibraryScore(Card card)
    {      
      return CalculateCardInGraveyardScore(card) - 1;
    }    
  }
}