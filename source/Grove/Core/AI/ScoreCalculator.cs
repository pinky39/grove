namespace Grove.AI
{
  using System;
  using System.Linq;

  public static class ScoreCalculator
  {
    public static readonly int HiddenCardInHandScore = 220;

    public static int CalculateTapPenalty(Card card, TurnInfo turnInfo)
    {
      if (card.Is().Land)
      {
        if (card.Controller.IsActive)
        {
          if (turnInfo.Step == Step.Upkeep)
            return 10;

          return 2;
        }
      }

      return 1;
    }

    public static int CalculateDiscardScore(Card card, bool isSearchInProgress)
    {
      if (isSearchInProgress && card.IsVisibleToSearchingPlayer == false)
      {
        return HiddenCardInHandScore;
      }

      // the lower the score, the more likely card will be discarded                                    
      var hand = card.Controller.Hand;
      var battlefield = card.Controller.Battlefield;

      if (card.Is().Land)
      {
        var landHandCount = hand.Count(x => x.Is().Land);
        var landBattlefieldCount = battlefield.Count(x => x.Is().Land);

        if ((landHandCount + landBattlefieldCount) < 4)
          return int.MaxValue;

        if (landHandCount < 2)
          return int.MaxValue;

        return 0 - 2*landBattlefieldCount;
      }

      return card.ConvertedCost;
    }

    public static int CalculateLifeScore(int life)
    {
      var score = 5000;

      if (life > 20)
        return score + (life - 20)*40;

      if (life <= 0)
      {
        return -1000 + 500 * life;
      }

      return score + Scores.LifeToScore[life];
    }

    public static int CalculatePermanentScore(Card permanent)
    {
      var score = 0;

      if (permanent.OverrideScore.Battlefield.HasValue)
        return permanent.OverrideScore.Battlefield.Value;

      if (permanent.Level > 0)
        score += 10*permanent.Level.Value;      

      if (permanent.ManaCost != null)
      {
        score += CalculatePermanentScoreFromManaCost(permanent);

        if (permanent.Is().Creature)
        {
          score += (permanent.Power.Value*10 + permanent.Toughness.Value*3);

          if (permanent.HasSummoningSickness)
            score -= 1;
        }        
      }
      else if (permanent.Is().Creature)
      {
        score += CalculatePermanentScoreFromPowerToughness(permanent.Power.Value, permanent.Toughness.Value);
      }
      else if (permanent.Is().Land)
      {
        score += GetLandOnBattlefieldScore(permanent);
        if (!permanent.Is().BasicLand)
          score += 10;
      }

      if (permanent.CountersCount() > 0)
      {
        score += permanent.CountersCount()*10;
      }

      return score;
    }

    private static int GetLandOnBattlefieldScore(Card land)
    {
      var landCount = land.Controller.Battlefield.Lands.Count();
      return landCount > 6 ? 350 : Scores.LandsOnBattlefieldToLandScore[landCount];
    }

    private static int CalculatePermanentScoreFromManaCost(Card permanent)
    {
      var converted = Math.Min(7, permanent.ManaCost.Converted);

      if (permanent.Has().Haste && converted > 0)
      {
        converted--;
      }

      return permanent.Has().Echo
        ? Scores.ManaCostToScoreEcho[converted]
        : Scores.ManaCostToScore[converted];
    }

    private static int CalculateCardInHandScoreFromManaCost(ManaAmount mana)
    {
      var converted = Math.Min(7, mana.Converted);
      var score = Scores.ManaCostToScore[converted] - 100;
      return score > 120 ? score : 120;
    }

    private static int CalculatePermanentScoreFromPowerToughness(int power, int toughness)
    {
      var powerToughness = power + toughness;

      if (powerToughness < 0)
        powerToughness = 0;
      else if (powerToughness > 10)
        powerToughness = 10;

      return Scores.PowerToughnessToScore[powerToughness];
    }

    public static int CalculateCardInHandScore(Card card, bool isSearchInProgress)
    {
      if (isSearchInProgress && card.IsVisibleToSearchingPlayer == false)
      {
        return HiddenCardInHandScore;
      }

      if (card.OverrideScore.Hand.HasValue)
        return card.OverrideScore.Hand.Value;

      if (card.ManaCost == null || card.ManaCost.Converted == 0)
      {
        return Scores.LandInHandCost;
      }

      return CalculateCardInHandScoreFromManaCost(card.ManaCost);
    }

    public static int CalculateCardInGraveyardScore(Card card)
    {
      if (card.OverrideScore.Graveyard.HasValue)
        return card.OverrideScore.Graveyard.Value;

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
      if (card.OverrideScore.Library.HasValue)
        return card.OverrideScore.Library.Value;

      return CalculateCardInGraveyardScore(card) - 1;
    }
  }
}