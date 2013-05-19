namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Gameplay;
  using Gameplay.Characteristics;

  public class DeckBuilder
  {
    private const int MinCreatureCount = 12;
    private const int MaxCreatureCount = 18;
    private const int DeckCardCount = 40;    

    private static readonly LandCountRule[] LandCountRules = new[]
      {
        new LandCountRule {Cost = 2.5, Count = 16},
        new LandCountRule {Cost = 3.0, Count = 17},
        new LandCountRule {Cost = 3.5, Count = 18},
      };

    private static readonly int MinSpellCount = DeckCardCount - LandCountRules[2].Count;
    private static readonly int MaxSpellCount = DeckCardCount - LandCountRules[0].Count;
    private readonly CardsDatabase _cardsDatabase;
    private readonly DeckEvaluator _deckEvaluator;

    public DeckBuilder(CardsDatabase cardsDatabase, DeckEvaluator deckEvaluator)
    {
      _cardsDatabase = cardsDatabase;
      _deckEvaluator = deckEvaluator;
    }

    public Deck BuildDeck(IEnumerable<string> cardNames, CardRatings cardRatings)
    {
      var candidates = BuildDecks(cardNames, cardRatings);
      return _deckEvaluator.GetBestDeck(candidates);
    }

    private List<Deck> BuildDecks(IEnumerable<string> cardNames, CardRatings cardRatings)
    {
      var cards = cardNames.Select(x => _cardsDatabase.CreateCard(x)).ToList();
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

      return decks.Select(d => new Deck(d.Select(x => x.Name))).ToList();
    }

    private void AddLands(List<Card> deck, CardRatings cardRatings)
    {
      var minimalLandCount = DeckCardCount - deck.Count;
      var landCount = Math.Max(minimalLandCount, CalculateOptimalLandCount(deck));

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

      var i = 0;
      while (roundingError > 0)
      {
        if (distribution[i] > 0)
        {
          distribution[i]++;
          roundingError--;
        }

        i++;
      }            

      Debug.Assert(distribution.Sum() == landCount);
      return distribution;
    }

    private void AddLands(string name, int count, List<Card> deck)
    {
      for (var i = 0; i < count; i++)
      {
        deck.Add(_cardsDatabase.CreateCard(name));
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

    private static int CalculateOptimalLandCount(List<Card> deckNoLands)
    {
      var avarageCost = deckNoLands.Sum(x => x.ConvertedCost)/deckNoLands.Count;

      foreach (var rule in LandCountRules)
      {
        if (avarageCost <= rule.Cost)
          return rule.Count;
      }

      return LandCountRules.Last().Count;
    }

    private static List<Card> BuildDeckNoLands(IEnumerable<Card> cards, CardRatings cardRatings)
    {
      var allCreatures = cards
        .Where(x => x.Is().Creature)
        .OrderByDescending(x => cardRatings.GetRating(x.Name))
        .ToList();

      var creaturesToInclude = allCreatures.Take(MinCreatureCount);
      var creaturesToConsider = allCreatures.Skip(MinCreatureCount).Take(MaxCreatureCount - MinCreatureCount).ToList();

      var cardsToConsider = cards
        .Where(x => !x.Is().Creature)
        .Concat(creaturesToConsider)
        .OrderByDescending(x => cardRatings.GetRating(x.Name))
        .ToList();

      var result = creaturesToInclude.ToList();

      result.AddRange(cardsToConsider
        .Take(MaxSpellCount - result.Count));


      if (result.Count < MinSpellCount)
        return null;
      
      return result;
    }

    private class LandCountRule
    {
      public double Cost;
      public int Count;
    }
  }
}