namespace Grove.Gameplay.AI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

  public static class DeckBuilder
  {
    private const int MinCreatureCount = 14;
    private const int MaxCreatureCount = 18;
    private const int DeckCardCount = 40;
    private const int MinSplashColorCardCount = 5;
    private const double MinNonBasicLandRating = 3.2;

    private static readonly LandCountRule[] LandCountRules = new[]
      {
        new LandCountRule {Cost = 2.5, Count = 16},
        new LandCountRule {Cost = 3.0, Count = 17},
        new LandCountRule {Cost = 3.5, Count = 18},
      };

    private const int MinSpellCount = 21;
    private static readonly int MaxSpellCount = DeckCardCount - LandCountRules[0].Count;

    public static Deck BuildDeck(IEnumerable<CardInfo> library, CardRatings cardRatings)
    {
      var cards = library.Select(x => Gameplay.Cards.All[x.Name])
        .ToList();

      var candidates = BuildDecks(cards, cardRatings)
        .Select(x => CreateDeckFromCardList(x, library))
        .ToList();

      return DeckEvaluator.GetBestDeck(candidates);
    }

    private static Deck CreateDeckFromCardList(List<Card> cards, IEnumerable<CardInfo> library)
    {
      var deck = new Deck();

      foreach (var card in cards)
      {
        var info = library.FirstOrDefault(x => x.Name.Equals(card.Name)) ??
          new CardInfo(card.Name, Rarity.C); // basic lands are not included in library

        deck.AddCard(info);
      }

      return deck;
    }

    private static List<List<Card>> BuildDecks(IEnumerable<Card> cards, CardRatings cardRatings)
    {
      var decks = new List<List<Card>>();

      for (var i = CardColor.White; i <= CardColor.Green; i++)
      {
        var cardWithColorI = cards
          .Where(x => x.IsMultiColored == false)
          .Where(x => x.HasColor(i) || x.HasColor(CardColor.Colorless) || x.Is().Land)
          .ToList();

        var monoColoredDeck = TryBuildDeck(cardWithColorI, cardRatings);

        if (monoColoredDeck != null)
          decks.Add(monoColoredDeck);

        for (var j = i + 1; j <= CardColor.Green; j++)
        {
          var dual = cards
            .Where(x => x.HasColor(i) || x.HasColor(j) || x.HasColor(CardColor.Colorless) || x.Is().Land)
            .ToList();

          var firstcolorCount = dual.Count(x => Gameplay.Cards.All[x.Name].HasColor(i));
          var secondcolorCount = dual.Count(x => Gameplay.Cards.All[x.Name].HasColor(j));

          if (firstcolorCount >= MinSplashColorCardCount && secondcolorCount >= MinSplashColorCardCount)
          {
            var dualDeck = TryBuildDeck(dual, cardRatings);

            if (dualDeck != null)
              decks.Add(dualDeck);
          }
        }
      }

      return decks;
    }

    private static List<Card> AddLands(List<Card> deck, List<Card> library, CardRatings cardRatings)
    {
      var minimalLandCount = DeckCardCount - deck.Count;
      var optimalLandCount = CalculateOptimalLandCount(deck);
      var landCount = Math.Max(minimalLandCount, optimalLandCount);

      var deckSize = landCount + deck.Count;

      if (deckSize > DeckCardCount)
      {
        RemoveWorstCards(
          deck,
          count: deckSize - DeckCardCount,
          cardRatings: cardRatings);
      }

      var distribution = GetLandsDistribution(deck, landCount);

      AddNonBasicLands(distribution, library, deck, cardRatings);

      AddCards("Plains", distribution[0], deck);
      AddCards("Island", distribution[1], deck);
      AddCards("Swamp", distribution[2], deck);
      AddCards("Mountain", distribution[3], deck);
      AddCards("Forest", distribution[4], deck);

      return deck;
    }

    private static void AddNonBasicLands(List<int> distribution, List<Card> library, List<Card> deck,
      CardRatings cardRatings)
    {
      var nonBasicLands = library.Where(x => x.Is().Land).ToList();
      var deckColors = distribution.Select((x, i) => x > 0 ? i : -1).Where(x => x >= 0).ToList();
      var maxCountColor = distribution.IndexOf(distribution.Max());

      var cardsToRemoveCount = 0;
      var landsToReplaceBasic = new List<Card>();

      foreach (var land in nonBasicLands)
      {
        var rating = cardRatings.GetRating(land.Name);
        if (rating < MinNonBasicLandRating)
          continue;

        var landColors = land.ProducableManaColors;

        if (landColors.Count == 0)
        {
          deck.Add(land);
          cardsToRemoveCount++;
        }
        if (landColors.Count == 1)
        {
          if (deckColors.Contains(landColors[0]))
          {
            distribution[landColors[0]]--;
            landsToReplaceBasic.Add(land);
          }
          else if (landColors[0] == (int) CardColor.Colorless)
          {
            if (deckColors.Count > 1)
            {
              deck.Add(land);
              cardsToRemoveCount++;
            }
            else
            {
              distribution[maxCountColor]--;
              landsToReplaceBasic.Add(land);
            }
          }
        }
        else if (landColors.Count > 1)
        {
          var matchCount = landColors.Count(deckColors.Contains);

          if (matchCount >= 2)
          {
            distribution[maxCountColor]--;
            landsToReplaceBasic.Add(land);
          }
        }
      }

      if (cardsToRemoveCount > 0)
      {
        // Some lands do not replace existing lands
        // e.g if they do not produce mana.
        // For each such land remove worst cards from deck
        // so that deck count will be 40.
        RemoveWorstCards(deck, cardsToRemoveCount, cardRatings);
      }

      deck.AddRange(landsToReplaceBasic);
    }

    private static List<int> GetLandsDistribution(IEnumerable<Card> noLandsDeck, int landCount)
    {
      var manaCounts = new[] {0, 0, 0, 0, 0};

      foreach (var card in noLandsDeck.Select(x => Gameplay.Cards.All[x.Name]))
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
      var distribution = manaCounts.Select(x => landCount*x/totalManaCount).ToList();
      var roundedCount = distribution.Sum();
      var roundingError = landCount - roundedCount;

      var i = 0;
      while (roundingError > 0)
      {
        var minLandCount = distribution.Where(x => x > 0).Min();

        for (var j = 0; j < distribution.Count; j++)
        {
          if (distribution[j] == minLandCount)
          {
            distribution[j]++;
            roundingError--;
            break;
          }
        }

        i++;
      }

      Asrt.True(distribution.Sum() == landCount, "Land distribution sum should equal land count.");
      return distribution;
    }

    private static void AddCards(string name, int count, List<Card> deck)
    {
      for (var i = 0; i < count; i++)
      {
        deck.Add(Gameplay.Cards.All[name]);
      }
    }

    private static void RemoveWorstCards(List<Card> deckNoLands, int count, CardRatings cardRatings)
    {
      var cardsToRemove = deckNoLands
        .OrderBy(x => cardRatings.GetRating(x.Name))
        .Take(count);

      foreach (var card in cardsToRemove)
      {
        deckNoLands.Remove(card);
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

    private static List<Card> TryBuildDeck(List<Card> cards, CardRatings cardRatings)
    {
      var deck = new List<Card>();

      var creatures = cards
        .Where(x => x.Is().Creature)
        .OrderByDescending(x => cardRatings.GetRating(x.Name))
        .ToList();

      var bestCreatures = creatures
        .Take(MinCreatureCount);

      var otherCreatures = creatures.Skip(MinCreatureCount)
        .Take(MaxCreatureCount - MinCreatureCount);

      var spellsAndOtherCreatures = cards
        .Where(x => !x.Is().Creature && !x.Is().Land)
        .Concat(otherCreatures)
        .OrderByDescending(x => cardRatings.GetRating(x.Name));

      deck.AddRange(bestCreatures);
      deck.AddRange(spellsAndOtherCreatures.Take(MaxSpellCount - deck.Count));


      if (deck.Count < MinSpellCount)
        return null;


      return AddLands(deck, cards, cardRatings);
    }


    private class LandCountRule
    {
      public double Cost;
      public int Count;
    }
  }
}