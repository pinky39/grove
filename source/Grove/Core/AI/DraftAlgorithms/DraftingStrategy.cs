namespace Grove.AI.DraftAlgorithms
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public abstract class DraftingStrategy : IDraftingStrategy
  {
    private readonly CardRatings _ratings;
    
    protected List<double> ManaCurveCreatureBonuses;
    protected readonly List<Card> DraftedCards = new List<Card>();
    protected List<Card> PlayableDraftedCards = new List<Card>();
    protected readonly ColorScore[] In = InitializeScores();
    protected readonly ColorScore[] Out = InitializeScores();
    protected CardColor PrimaryColor;
    protected CardColor? SecondaryColor;

    protected virtual double PlayableThreshold { get; } = 1.2;
    protected virtual int GiveBonusUntilCreatureCountIs { get; } = 18;
    protected virtual double CreatureCountBonus { get; } = 0.5d;
    protected virtual double ManaCurveCreatureBonus { get; } = 0.5d;

    protected virtual int[] CreaturesManaCurve { get; } = new[]
    {
      5, // up to 2
      6, // up to 3
      3, // up to 4
      2, // up to 5
      2  // 6 or more
    };

    protected DraftingStrategy(CardRatings ratings)
    {
      _ratings = ratings;
    }

    public CardInfo PickCard(List<CardInfo> boosterInfo, int round)
    {
      ManaCurveCreatureBonuses = GetManaCurveBonuses(round);
      var booster = GetBoosterCards(boosterInfo);

      if (booster.Count < 15)
      {
        UpdateIn(booster, round);
      }

      var chosenCard = ChooseCard(booster, round);

      DraftedCards.Add(chosenCard);
      PlayableDraftedCards = GetPlayableCards(DraftedCards);

      UpdateOut(Out, booster, chosenCard, round);

      return boosterInfo.First(x => x.Name.Equals(chosenCard.Name));
    }

    protected abstract Card ChooseCard(List<Card> booster, int round);

    protected Card GetBestCard(List<Card> booster, int round)
    {
      return booster.OrderByDescending(x => GetRating(x, round)).First();
    }

    protected static ColorScore[] InitializeScores()
    {
      return new[]
        {
          new ColorScore {Color = CardColor.White, Score = 0d},
          new ColorScore {Color = CardColor.Blue, Score = 0d},
          new ColorScore {Color = CardColor.Black, Score = 0d},
          new ColorScore {Color = CardColor.Red, Score = 0d},
          new ColorScore {Color = CardColor.Green, Score = 0d},
        };
    }

    private Dictionary<CardColor, double> CalculateBoosterColorScores(IEnumerable<Card> booster, int round)
    {
      return booster
        .Where(x => !x.IsColorless())
        .Where(x => x.Colors.Length == 1)
        .GroupBy(x => x.Colors[0])
        .Select(x => new
          {
            Color = x.Key,
            Score = x.Max(c => GetRating(c, round))
          })
        .ToDictionary(x => x.Color, x => x.Score);
    }

    protected void UpdateColorScores(ColorScore[] colorScores, Dictionary<CardColor, double> boosterScores)
    {
      for (var i = 0; i < colorScores.Length; i++)
      {
        if (boosterScores.ContainsKey(colorScores[i].Color) && boosterScores[colorScores[i].Color] > PlayableThreshold)
        {
          colorScores[i].Score += boosterScores[colorScores[i].Color] - PlayableThreshold;
        }
      }
    }

    protected double GetRating(Card card, int round, bool usePlayerContext = true)
    {
      var baseRating = _ratings.GetRating(card.Name);

      if (usePlayerContext)
      {
        if (card.Is().Creature && baseRating > PlayableThreshold) 
        {
          if (PlayableDraftedCards.Count(x => x.Is().Creature) < (GiveBonusUntilCreatureCountIs / (4 - round)))
          {
            baseRating += CreatureCountBonus;
          }

          if (card.ManaCost != null)
          {
            var cost = GetConvertedManaCurveCost(card);
            baseRating += ManaCurveCreatureBonuses[cost - 2];
          }
        }                
      }
      
      return baseRating;
    }

    private void UpdateIn(IEnumerable<Card> booster, int round)
    {
      var boosterScores = CalculateBoosterColorScores(booster, round);
      UpdateColorScores(In, boosterScores);
    }

    protected void UpdateOut(ColorScore[] outList, IEnumerable<Card> booster, Card pickedCard, int round)
    {
      var boosterWithoutPickedCard = booster.ToList();
      boosterWithoutPickedCard.Remove(pickedCard);

      var boosterScores = CalculateBoosterColorScores(boosterWithoutPickedCard, round);
      UpdateColorScores(outList, boosterScores);
    }

    protected List<Card> GetBoosterCards(IEnumerable<CardInfo> boosterInfo)
    {
      return boosterInfo.Select(x => Cards.All[x.Name]).ToList();
    }

    protected Card GetBestCardOfChosenColor(CardColor chosenColor, IEnumerable<Card> cards, int round)
    {
      return cards
        .Where(x => x.Colors.Length == 1)
        .Where(x => x.Colors[0] == chosenColor)
        .OrderByDescending(c => GetRating(c, round))
        .FirstOrDefault();
    }

    protected static ColorScore[] CloneScores(IEnumerable<ColorScore> scores)
    {
      return scores.Select(x => new ColorScore {Color = x.Color, Score = x.Score}).ToArray();
    }

    protected Card GetBestCardOfSingleColorOrColorless(IEnumerable<Card> cards, int round, CardColor? primaryColor = null)
    {
      return cards
        .Where(x => x.Colors.Length == 1 || (primaryColor.HasValue && IsUsableNonbasicLand(x, primaryColor.Value)))
        .OrderByDescending(c => GetRating(c, round))
        .FirstOrDefault();
    }

    protected static int GetColorRank(CardColor cardColor, ColorScore[] scores)
    {
      var i = 1;
      foreach (var colorScore in scores.OrderByDescending(x => x.Score))
      {
        if (cardColor == colorScore.Color)
          return i;
      }

      return scores.Length;
    }

    protected Card PickBestPrimarySecondaryOrColorless(List<Card> booster, int round)
    {
      if (SecondaryColor == null)
      {
        var chosen = GetBestCardOfSingleColorOrColorless(booster, round, PrimaryColor);

        if (chosen == null)
          return GetBestCard(booster, round);

        if (chosen.IsColorless() || chosen.HasColor(PrimaryColor))
          return chosen;

        SecondaryColor = chosen.Colors[0];
        return chosen;
      }

      return GetBestCardOfChosenColorsOrColorless(
        color1: PrimaryColor,
        color2: SecondaryColor.Value,
        cards: booster, 
        round: round)      
        ??
        GetBestCard(booster, round);
    }

    protected Card GetBestCardOfChosenColorsOrColorless(CardColor color1, IEnumerable<Card> cards, int round,
      CardColor? color2 = null)
    {
      return cards
        .Where(card =>
          {
            if (IsUsableNonbasicLand(card, color1, color2))
            {
              return true;
            }

            if (card.HasColor(CardColor.Colorless))
              return true;

            return card.Colors.All(c =>
              {
                if (color2 == null)
                {
                  return c == color1;
                }

                return c == color1 || c == color2;
              });
          })
        .OrderByDescending(c => GetRating(c, round))
        .FirstOrDefault();
    }

    private bool IsUsableNonbasicLand(Card card, CardColor color1, CardColor? color2 = null)
    {
      if (!card.Is().Land)
        return false;

      var landColors = card.ProducableManaColors;

      if (landColors.Count == 0)
        return true;

      if (landColors.Count == 1)
      {
        if (landColors[0] == (int) CardColor.Colorless)
          return true;

        if (landColors[0] == (int) color1)
          return true;

        if (color2.HasValue && landColors[0] == (int) color2.Value)
          return true;

        return false;
      }

      if (!color2.HasValue)
        return false;

      return landColors.Contains((int) color1) &&
        landColors.Contains((int) color2.Value);
    }

    private int GetConvertedManaCurveCost(Card card)
    {
      var converted = card.ManaCost.Converted;

      if (converted <= 2)
        return 2;

      if (converted >= 6)
        return 6;

      return converted;
    }

    protected List<double> GetManaCurveBonuses(int round)
    {
      var existingCurve =
        PlayableDraftedCards
          .Where(x => x.ManaCost != null)
          .GroupBy(GetConvertedManaCurveCost)
          .Select(x => new
          {
            Cost = x.Key,
            Count = x.Count()
          })
          .OrderBy(x => x.Cost)
          .ToArray();

      var idealRoundCurve = CreaturesManaCurve
        .Select(x => Math.Ceiling(x / (4d - round)))
        .ToArray();

      var bonuses = new List<double>();

      for (int i = 0; i < idealRoundCurve.Length; i++)
      {
        var existingCount = existingCurve
          .FirstOrDefault(x => x.Cost == (i + 2))?.Count ?? 0;

        var idealCount = idealRoundCurve[i];

        var missingCount = idealCount - existingCount;

        var bonus = missingCount > 0
          ? ManaCurveCreatureBonus * missingCount
          : 0d;

        bonuses.Add(bonus);
      }
      return bonuses;
    }

    private List<Card> GetPlayableCards(List<Card> cards)
    {
      var playable = cards
        .Where(c =>
        {
          if (_ratings.GetRating(c.Name) < PlayableThreshold)
            return false;

          if (c.IsColorless())
            return true;

          if (c.Colors.Length == 1)
          {
            return SecondaryColor == null || c.Colors[0] == PrimaryColor || c.Colors[0] == SecondaryColor;
          }

          if (c.Colors.Length == 2)
          {
            return c.Colors.Contains(PrimaryColor) &&
              (SecondaryColor == null || c.Colors.Contains(SecondaryColor.Value));
          }

          return false;
        })
        .ToList();

      return playable;
    }
  }
}