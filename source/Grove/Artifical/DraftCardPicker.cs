namespace Grove.Artifical
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Tournaments;
  using Infrastructure;

  public class DraftCardPicker : IDraftCardPicker
  {
    private const int CreatureCount = 16;
    private static readonly double[] ColorBonuses = new[] {2d, 5d, 10d};
    private static readonly double[] CreatureBonuses = new[] {1d, 2d, 4d};
    private readonly CardsDictionary _c;

    public DraftCardPicker(CardsDictionary c)
    {
      _c = c;
    }

    public CardInfo PickCard(List<CardInfo> draftedCards, List<CardInfo> booster, int round, CardRatings ratings)
    {
      var colorScores = new Dictionary<CardColor, double>
        {
          {CardColor.White, CalculateColorScore(CardColor.White, draftedCards, ratings)},
          {CardColor.Blue, CalculateColorScore(CardColor.Blue, draftedCards, ratings)},
          {CardColor.Black, CalculateColorScore(CardColor.Black, draftedCards, ratings)},
          {CardColor.Red, CalculateColorScore(CardColor.Red, draftedCards, ratings)},
          {CardColor.Green, CalculateColorScore(CardColor.Green, draftedCards, ratings)},
        };

      var creatureCount = draftedCards.Count(x => _c[x.Name].Is().Creature);

      var top2Colors = colorScores
        .OrderByDescending(x => x.Value)
        .ThenBy(x => RandomEx.Next())
        .Take(2).Select(x => x.Key).ToArray();

      return booster.Select(x =>
        {
          var baseRating = ratings.GetRating(x.Name);
          return new
            {
              Card = x,
              Score = baseRating > 3
                ? baseRating + CalculateScoreAdjustmentFactor(x, round, creatureCount, top2Colors)
                : baseRating
            };
        })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card)
        .First();
    }

    private double CalculateScoreAdjustmentFactor(CardInfo cardInfo, int round, int creatureCount,
      CardColor[] top2Colors)
    {
      var card = _c[cardInfo.Name];

      if (card.Is().Land)
      {
        return -2;
      }

      var colorFactor = 1d;

      if (card.Colors.All(x => top2Colors.Contains(x)) || card.HasColor(CardColor.Colorless))
      {
        colorFactor += ColorBonuses[round - 1];
      }

      var cardTypeFactor = 1d;

      if (card.Is().Creature && creatureCount < CreatureCount)
      {
        cardTypeFactor += CreatureBonuses[round - 1]*(CreatureCount - creatureCount)/20d;
      }

      return colorFactor*cardTypeFactor;
    }

    private double CalculateColorScore(CardColor color, IEnumerable<CardInfo> cards, CardRatings ratings)
    {
      return cards
        .Where(x => _c[x.Name].HasColor(color))
        .Select(x => ratings.GetRating(x.Name))
        .Sum();
    }
  }
}