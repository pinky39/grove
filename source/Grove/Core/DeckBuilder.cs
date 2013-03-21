namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Mana;

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

    private static readonly Func<Card, bool>[] ColorFilters = new Func<Card, bool>[]
      {
        x => x.HasColors(ManaColors.White),
        x => x.HasColors(ManaColors.Blue),
        x => x.HasColors(ManaColors.Black),
        x => x.HasColors(ManaColors.Red),
        x => x.HasColors(ManaColors.Green),
      };

    private readonly CardDatabase _cardDatabase;

    public DeckBuilder(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
    }

    public List<List<string>> BuildDecks(List<string> cardNames, CardRatings cardRatings)
    {
      var cards = cardNames.Select(x => _cardDatabase.CreateCard(x)).ToList();

      var decks = new List<List<Card>>();

      // create mono and dual colored decks
      for (var i = 0; i < 5; i++)
      {
        var mono = cards
          .Where(x => !x.IsMultiColored)
          .Where(x => ColorFilters[i](x) || x.HasColors(ManaColors.Colorless))
          .ToList();

        var monoDeck = BuildDeckNoLands(mono, cardRatings);
        if (monoDeck != null)
          decks.Add(monoDeck);

        for (var j = i + 1; j < 5; j++)
        {
          var dual = cards
            .Where(x => ColorFilters[i](x) || ColorFilters[j](x) || x.HasColors(ManaColors.Colorless))
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

      deck.AddRange(CreateLands("Plains", distribution[0]));
      deck.AddRange(CreateLands("Island", distribution[1]));
      deck.AddRange(CreateLands("Swamp", distribution[2]));
      deck.AddRange(CreateLands("Mountain", distribution[3]));
      deck.AddRange(CreateLands("Forest", distribution[4]));
    }

    private static int[] GetLandsDistribution(int landCount, List<Card> deck)
    {
      var manaCounts = new ColoredManaCounts();

      foreach (var card in deck)
      {
        manaCounts.White += card.ManaCost.Count(x => x.HasColor(ManaColors.White));
        manaCounts.Blue += card.ManaCost.Count(x => x.HasColor(ManaColors.Blue));
        manaCounts.Black += card.ManaCost.Count(x => x.HasColor(ManaColors.Black));
        manaCounts.Red += card.ManaCost.Count(x => x.HasColor(ManaColors.Red));
        manaCounts.Green += card.ManaCost.Count(x => x.HasColor(ManaColors.Green));
      }

      Func<int, int> getCount = colorCount => landCount*colorCount/manaCounts.Total;

      var distribution = new[]
        {
          getCount(manaCounts.White),
          getCount(manaCounts.Blue),
          getCount(manaCounts.Black),
          getCount(manaCounts.Red),
          getCount(manaCounts.Green)
        };

      var roundedCount = distribution.Sum();
      var landsToDistribute = landCount - roundedCount;

      for (var i = 0; i < distribution.Length; i++)
      {
        if (distribution[i] > 0)
          distribution[i]++;

        landsToDistribute--;

        if (landsToDistribute == 0)
          break;
      }

      return distribution;
    }

    private IEnumerable<Card> CreateLands(string name, double count)
    {
      for (var i = 0; i < count; i++)
      {
        yield return _cardDatabase.CreateCard(name);
      }
    }

    private void RemoveWorstCards(int count, List<Card> deck, CardRatings cardRatings)
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

    private List<Card> BuildDeckNoLands(IEnumerable<Card> cards, CardRatings cardRatings)
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

    private class ColoredManaCounts
    {
      public int Black;
      public int Blue;
      public int Green;
      public int Red;
      public int White;

      public int Total { get { return White + Blue + Black + Red + Green; } }
    }

    private class LandCountRule
    {
      public double Cost;
      public int Count;
    }
  }
}