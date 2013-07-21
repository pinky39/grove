namespace Grove.Artifical
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Characteristics;

  public class CardDrafter
  {
    private const int MinCreatureCount = 14;
    private readonly CardsDictionary _c;

    public CardDrafter(CardsDictionary c)
    {
      _c = c;
    }

    public CardInfo DraftCard(List<CardInfo> draftedCards, List<CardInfo> booster, int round, CardRatings ratings)
    {
      var colorScores = new Dictionary<CardColor, double>
        {
          {CardColor.White, CalculateColorScore(CardColor.White, draftedCards, ratings)},
          {CardColor.Blue, CalculateColorScore(CardColor.Blue, draftedCards, ratings)},
          {CardColor.Black, CalculateColorScore(CardColor.Black, draftedCards, ratings)},
          {CardColor.Red, CalculateColorScore(CardColor.Red, draftedCards, ratings)},
          {CardColor.Green, CalculateColorScore(CardColor.Green, draftedCards, ratings)},
        };

      var totalScore = draftedCards.Select(x => ratings.GetRating(x.Name)).Sum();
      var creatureCount = draftedCards.Count(x => _c[x.Name].Is().Creature);

      return booster.Select(x => new
        {
          Card = x,
          Score = ratings.GetRating(x.Name) +
            CalculateScoreAdjustmentFactor(x, round, creatureCount, colorScores, totalScore)
        })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card)
        .First();
    }

    private double CalculateScoreAdjustmentFactor(CardInfo cardInfo, int round, int creatureCount,
      Dictionary<CardColor, double> colorScores, double totalScore)
    {
      var card = _c[cardInfo.Name];

      if (card.Is().Land)
      {
        // todo land drafting algorithm
        return -2;
      }

      double colorFactor;      
      
      if (card.HasColor(CardColor.Colorless))
      {       
        // can be used in each deck
        colorFactor = 0.5*round;
      }
      else
      {
        // take the smallest bonus if multicolored
        colorFactor = card.Colors
          .Select(x => 0.5 * round * colorScores[x]/totalScore)
          .OrderBy(x => x).First();
      }

      var creatureFactor = 0.0d;

      if (card.Is().Creature && creatureCount < MinCreatureCount)
      {
        creatureFactor = round*(MinCreatureCount - creatureCount)/24.0d;
      }

      return colorFactor + creatureFactor;
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