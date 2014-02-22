namespace Grove.Gameplay.AI.DraftAlgorithms
{
  using System.Collections.Generic;
  using System.Linq;

  public abstract class DraftingStrategy : IDraftingStrategy
  {
    private readonly CardRatings _ratings;
    protected const double PlayableThreshold = 3.1;
    protected const int MinimalCreatures = 12;
    protected const double CreaturesBonus = 0.2;
    protected readonly List<Card> DraftedCards = new List<Card>();
    protected readonly ColorScore[] In = InitializeScores();
    protected readonly ColorScore[] Out = InitializeScores();
    protected CardColor PrimaryColor;
    protected CardColor? SecondaryColor;

    protected DraftingStrategy(CardRatings ratings)
    {
      _ratings = ratings;
    }

    public CardInfo PickCard(List<CardInfo> boosterInfo, int round)
    {
      var booster = GetBoosterCards(boosterInfo);

      if (booster.Count < 15)
      {
        UpdateIn(booster);
      }

      var chosenCard = ChooseCard(booster, round);

      DraftedCards.Add(chosenCard);
      UpdateOut(Out, booster, chosenCard);

      return boosterInfo.First(x => x.Name.Equals(chosenCard.Name));
    }

    protected abstract Card ChooseCard(List<Card> booster, int round);

    protected Card GetBestCard(List<Card> booster)
    {
      return booster.OrderByDescending(GetRating).First();
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

    private Dictionary<CardColor, double> CalculateBoosterColorScores(IEnumerable<Card> booster)
    {
      return booster
        .Where(x => !x.IsColorless())
        .Where(x => x.Colors.Length == 1)
        .GroupBy(x => x.Colors[0])
        .Select(x => new
          {
            Color = x.Key,
            Score = x.Max(c => GetRating(c))
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

    protected double GetRating(Card card)
    {
      var baseRating = _ratings.GetRating(card.Name);

      if (card.Is().Creature && DraftedCards.Count(x => x.Is().Creature) < MinimalCreatures)
        baseRating += CreaturesBonus;

      return baseRating;
    }

    private void UpdateIn(IEnumerable<Card> booster)
    {
      var boosterScores = CalculateBoosterColorScores(booster);
      UpdateColorScores(In, boosterScores);
    }

    protected void UpdateOut(ColorScore[] outList, IEnumerable<Card> booster, Card pickedCard)
    {
      var boosterWithoutPickedCard = booster.ToList();
      boosterWithoutPickedCard.Remove(pickedCard);

      var boosterScores = CalculateBoosterColorScores(boosterWithoutPickedCard);
      UpdateColorScores(outList, boosterScores);
    }

    protected List<Card> GetBoosterCards(IEnumerable<CardInfo> boosterInfo)
    {
      return boosterInfo.Select(x => Gameplay.Cards.All[x.Name]).ToList();
    }

    protected Card GetBestCardOfChosenColor(CardColor chosenColor, IEnumerable<Card> cards)
    {
      return cards
        .Where(x => x.Colors.Length == 1)
        .Where(x => x.Colors[0] == chosenColor)
        .OrderByDescending(GetRating)
        .FirstOrDefault();
    }

    protected static ColorScore[] CloneScores(IEnumerable<ColorScore> scores)
    {
      return scores.Select(x => new ColorScore {Color = x.Color, Score = x.Score}).ToArray();
    }

    protected Card GetBestCardOfSingleColorOrColorless(IEnumerable<Card> cards, CardColor? primaryColor = null)
    {
      return cards
        .Where(x => x.Colors.Length == 1 || (primaryColor.HasValue && IsUsableNonbasicLand(x, primaryColor.Value)))
        .OrderByDescending(GetRating)
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

    protected Card PickBestPrimarySecondaryOrColorless(List<Card> booster)
    {
      if (SecondaryColor == null)
      {
        var chosen = GetBestCardOfSingleColorOrColorless(booster, PrimaryColor);

        if (chosen == null)
          return GetBestCard(booster);

        if (chosen.IsColorless() || chosen.HasColor(PrimaryColor))
          return chosen;

        SecondaryColor = chosen.Colors[0];
        return chosen;
      }

      return GetBestCardOfChosenColorsOrColorless(
        color1: PrimaryColor,
        color2: SecondaryColor.Value,
        cards: booster)
        ??
        GetBestCard(booster);
    }

    protected Card GetBestCardOfChosenColorsOrColorless(CardColor color1, IEnumerable<Card> cards,
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
        .OrderByDescending(GetRating)
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
  }
}