namespace Grove.Artifical.DraftAlgorithms
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Tournaments;

  public class Forcing : IDraftingStrategy
  {
    private const double PlayableThreshold = 3.1;
    private const int MinimalCreatures = 12;
    private const double CreaturesBonus = 0.2;
    private readonly CardDatabase _c;
    private readonly List<Card> _draftedCards = new List<Card>();
    private readonly ColorScore[] _in = InitializeScores();
    private readonly ColorScore[] _out = InitializeScores();
    private readonly CardRatings _ratings;
    private CardColor _primaryForcingColor;
    private CardColor? _secondaryForcingColor;

    public Forcing(CardDatabase c, CardRatings ratings)
    {
      _c = c;
      _ratings = ratings;
    }

    public CardInfo PickCard(List<CardInfo> boosterInfo, int round)
    {
      Card chosenCard = null;

      var booster = GetBooster(boosterInfo);

      if (booster.Count < 15)
      {
        UpdateIn(booster);
      }

      chosenCard = ChooseCard(booster, round);

      _draftedCards.Add(chosenCard);
      UpdateOut(_out, booster, chosenCard);

      return boosterInfo.First(x => x.Name.Equals(chosenCard.Name));
    }

    private List<Card> GetBooster(IEnumerable<CardInfo> boosterInfo)
    {
      return boosterInfo.Select(x => _c[x.Name]).ToList();
    }

    private Card ChooseCard(List<Card> booster, int round)
    {
      Card pickedCard = null;

      switch (round)
      {
        case 1:

          pickedCard = booster.Count == 15
            ? PickFirstCardOfTheDraft(booster)
            : BlockPrimaryOrSnatchSecondary(booster);

          break;

        case 3:
        case 2:
          pickedCard = PickBestPrimarySecondaryOrColorless(booster);
          break;
      }

      return pickedCard;
    }

    private Card PickBestPrimarySecondaryOrColorless(List<Card> booster)
    {
      if (_secondaryForcingColor == null)
      {
        var chosen = GetBestCardOfSingleColorOrColorless(booster);

        if (chosen == null)
          return GetBestCard(booster);

        if (chosen.IsColorless() || chosen.HasColor(_primaryForcingColor))
          return chosen;

        _secondaryForcingColor = chosen.Colors[0];
        return chosen;
      }

      return GetBestCardOfChosenColorsOrColorless(_primaryForcingColor, _secondaryForcingColor.Value, booster) ??
        GetBestCard(booster);
    }

    private Card GetBestCard(List<Card> booster)
    {
      return booster.OrderByDescending(GetRating).First();
    }

    private double GetRating(Card card)
    {
      var baseRating = _ratings.GetRating(card.Name);

      if (card.Is().Creature && _draftedCards.Count(x => x.Is().Creature) < MinimalCreatures)
        baseRating += CreaturesBonus;

      return baseRating;
    }

    private Card BlockPrimaryOrSnatchSecondary(List<Card> booster)
    {
      var outListIfPrimaryIsPicked = CloneScores(_out);
      var outListIfPrimaryIsNotPicked = CloneScores(_out);

      var primaryPick = GetBestCardOfChosenColor(_primaryForcingColor, booster);

      var secondaryColor = _secondaryForcingColor.HasValue
        ? _secondaryForcingColor.Value
        : _in
          .Where(x => x.Color != _primaryForcingColor)
          .OrderByDescending(x => x.Score)
          .First()
          .Color;

      var secondaryPick = GetBestCardOfChosenColor(secondaryColor, booster);

      var primaryRating = primaryPick != null ? GetRating(primaryPick) : PlayableThreshold;
      var secondaryRating = secondaryPick != null ? GetRating(secondaryPick) : PlayableThreshold;

      if (primaryPick != null && primaryRating > PlayableThreshold)
      {
        if (secondaryPick == null)
        {
          return primaryPick;
        }

        UpdateOut(outListIfPrimaryIsPicked, booster, primaryPick);
        UpdateOut(outListIfPrimaryIsNotPicked, booster, secondaryPick);

        var notPickedRank = GetColorRank(_primaryForcingColor, outListIfPrimaryIsNotPicked);

        if (secondaryRating > primaryRating && notPickedRank > 2)
        {
          _secondaryForcingColor = secondaryPick.Colors[0];
          return secondaryPick;
        }

        return primaryPick;
      }

      if (secondaryPick != null && secondaryRating > PlayableThreshold)
      {
        _secondaryForcingColor = secondaryPick.Colors[0];
        return secondaryPick;
      }

      return PickBestColorlessOrOffcolor(booster);
    }

    private int GetColorRank(CardColor cardColor, ColorScore[] scores)
    {
      var i = 1;
      foreach (var colorScore in scores.OrderByDescending(x => x.Score))
      {
        if (cardColor == colorScore.Color)
          return i;
      }

      return scores.Length;
    }

    private Card GetBestCardOfChosenColorsOrColorless(CardColor color1, CardColor color2, IEnumerable<Card> cards)
    {
      return cards
        .Where(x =>
          {
            if (x.IsColorless())
              return true;

            return x.Colors.All(c => c == color1 || c == color2);
          })
        .OrderByDescending(GetRating)
        .FirstOrDefault();
    }

    private Card GetBestCardOfSingleColorOrColorless(IEnumerable<Card> cards)
    {
      return cards
        .Where(x => x.Colors.Length == 1)
        .OrderByDescending(GetRating)
        .FirstOrDefault();
    }

    private Card GetBestCardOfChosenColor(CardColor chosenColor, IEnumerable<Card> cards)
    {
      return cards
        .Where(x => x.Colors.Length == 1)
        .Where(x => x.Colors[0] == chosenColor)
        .OrderByDescending(GetRating)
        .FirstOrDefault();
    }

    private Card PickBestColorlessOrOffcolor(List<Card> booster)
    {
      return booster
        .OrderBy(x =>
          {
            if (x.IsColorless())
              return 0;

            return 1;
          })
        .ThenByDescending(GetRating)
        .First();
    }

    private static ColorScore[] CloneScores(IEnumerable<ColorScore> scores)
    {
      return scores.Select(x => new ColorScore {Color = x.Color, Score = x.Score}).ToArray();
    }

    private static ColorScore[] InitializeScores()
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

    private void UpdateIn(IEnumerable<Card> booster)
    {
      var boosterScores = CalculateBoosterColorScores(booster);
      UpdateColorScores(_in, boosterScores);
    }

    public void UpdateColorScores(ColorScore[] colorScores, Dictionary<CardColor, double> boosterScores)
    {
      for (var i = 0; i < colorScores.Length; i++)
      {
        if (boosterScores.ContainsKey(colorScores[i].Color) && boosterScores[colorScores[i].Color] > PlayableThreshold)
        {
          colorScores[i].Score += boosterScores[colorScores[i].Color] - PlayableThreshold;
        }
      }
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

    private void UpdateOut(ColorScore[] outList, IEnumerable<Card> booster, Card pickedCard)
    {
      var boosterWithoutPickedCard = booster.ToList();
      boosterWithoutPickedCard.Remove(pickedCard);

      var boosterScores = CalculateBoosterColorScores(boosterWithoutPickedCard);
      UpdateColorScores(outList, boosterScores);
    }

    private Card PickFirstCardOfTheDraft(IEnumerable<Card> booster)
    {
      var card = booster
        .Where(x => !x.IsColorless())
        .OrderByDescending(GetRating)
        .First();

      _primaryForcingColor = card.Colors[0];
      return card;
    }
  }

  public class ColorScore
  {
    public CardColor Color;
    public double Score;
  }
}