namespace Grove.Tournament
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;

  public class DeckBuilder
  {
    private const int MinCreatureCount = 15;
    private const int DeckCardCount = 40;

    private static readonly LandCountRule[] LandCountRules = new[]
      {
        new LandCountRule {Cost = 2.5, Count = 16},
        new LandCountRule {Cost = 3.0, Count = 17},
        new LandCountRule {Cost = 3.5, Count = 18},
      };

    private static readonly int MinSpellCount = DeckCardCount - LandCountRules[0].Count;
    private readonly CardDatabase _cardDatabase;

    public DeckBuilder(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
    }

    public List<List<string>> BuildDecks(List<string> cardNames, CardRatings cardRatings)
    {
      var cards = cardNames.Select(x => _cardDatabase.CreateCard(x)).ToList();
      var decks = new List<List<Card>>();

      for (var i = CardColor.White; i <= CardColor.Green; i++)
      {
        var mono = cards
          .Where(x => x.IsMultiColored == false)
          .Where(x => x.HasColor(i) || x.HasColor(CardColor.Colorless))
          .ToList();

        var monoDeck = BuildDeckNoLands(mono, cardRatings);

        if (monoDeck != null)
          decks.Add(monoDeck);

        for (var j = i + 1; j <= CardColor.Green; j++)
        {
          var dual = cards
            .Where(x => x.HasColor(i) || x.HasColor(j) || x.HasColor(CardColor.Colorless))
            .ToList();

          var dualDeck = BuildDeckNoLands(dual, cardRatings);

          if (dualDeck != null)
            decks.Add(dualDeck);
        }
      }

      foreach (var deck in decks)
      {
        AddLands(deck, cardRatings);
      }

      return decks.Select(d => d.Select(c => c.Name).ToList()).ToList();
    }

    private void AddLands(List<Card> deck, CardRatings cardRatings)
    {
      var landCount = CalculateOptimalLandCount(deck);

      var actualDeckSize = landCount + deck.Count;

      if (actualDeckSize > DeckCardCount)
      {
        RemoveWorstCards(actualDeckSize - DeckCardCount, deck, cardRatings);
      }

      var distribution = GetLandsDistribution(landCount, deck);

      AddLands("Plains", distribution[0], deck);
      AddLands("Island", distribution[1], deck);
      AddLands("Swamp", distribution[2], deck);
      AddLands("Mountain", distribution[3], deck);
      AddLands("Forest", distribution[4], deck);
    }

    private static int[] GetLandsDistribution(int landCount, IEnumerable<Card> deck)
    {
      var manaCounts = new[] {0, 0, 0, 0, 0};

      foreach (var card in deck)
      {
        foreach (var colorCount in card.ManaCost)
        {
          if (colorCount.Color.IsColorless)
            continue;

          foreach (var index in colorCount.Color.Indices)
          {
            manaCounts[index]++;
          }
        }
      }

      var totalManaCount = manaCounts.Sum();
      var distribution = manaCounts.Select(x => landCount*x/totalManaCount).ToArray();
      var roundedCount = distribution.Sum();
      var roundingError = landCount - roundedCount;

      for (var i = 0; i < roundingError; i++)
      {
        if (distribution[i] > 0)
          distribution[i]++;

        roundingError--;

        if (roundingError == 0)
          break;
      }

      return distribution;
    }

    private void AddLands(string name, int count, List<Card> deck)
    {
      for (var i = 0; i < count; i++)
      {
        deck.Add(_cardDatabase.CreateCard(name));
      }
    }

    private static void RemoveWorstCards(int count, List<Card> deck, CardRatings cardRatings)
    {
      var cardsToRemove = deck
        .OrderBy(x => cardRatings.GetRating(x.Name))
        .Take(count);

      foreach (var card in cardsToRemove)
      {
        deck.Remove(card);
      }
    }

    private static int CalculateOptimalLandCount(List<Card> deck)
    {
      var avarageCost = deck.Sum(x => x.ConvertedCost)/deck.Count;

      foreach (var rule in LandCountRules)
      {
        if (avarageCost <= rule.Cost)
          return rule.Count;
      }

      return LandCountRules.Last().Count;
    }

    private static List<Card> BuildDeckNoLands(IEnumerable<Card> cards, CardRatings cardRatings)
    {
      var creatures = cards
        .Where(x => x.Is().Creature)
        .OrderByDescending(x => cardRatings.GetRating(x.Name))
        .ToList();

      var other = cards
        .Where(x => !x.Is().Creature)
        .ToList();

      if (creatures.Count < MinCreatureCount)
        return null;

      if (creatures.Count + other.Count < MinSpellCount)
        return null;

      var result = creatures.Take(MinCreatureCount).ToList();

      other.AddRange(creatures.Skip(MinCreatureCount));

      result.AddRange(other
        .OrderByDescending(x => cardRatings.GetRating(x.Name))
        .Take(MinSpellCount - MinCreatureCount));

      return result;
    }

    private class LandCountRule
    {
      public double Cost;
      public int Count;
    }
  }
}