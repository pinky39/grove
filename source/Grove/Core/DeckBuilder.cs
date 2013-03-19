namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Mana;

  public class DeckBuilder
  {
    private readonly CardDatabase _cardDatabase;
    private readonly CardRatings _cardRatings;

    private const int MinCreatureCount = 15;    
    private const int DeckCardCount = 40;
    private const int LowLandCount = 16;
    private const int MedLandCount = 17;
    private const int HighLandCount = 18;    
    private const int MinSpellCount = DeckCardCount - LowLandCount;

    private static readonly Func<Card, bool>[] ColorFilters = new Func<Card, bool>[]
      {
        x => x.HasColors(ManaColors.White),
        x => x.HasColors(ManaColors.Blue),
        x => x.HasColors(ManaColors.Black),
        x => x.HasColors(ManaColors.Red),
        x => x.HasColors(ManaColors.Green),
    };

    public DeckBuilder(CardDatabase cardDatabase, CardRatings cardRatings)
    {
      _cardDatabase = cardDatabase;
      _cardRatings = cardRatings;
    }

    public List<List<string>> BuildDecks(List<string> cardNames)
    {
      var cards = cardNames.Select(x => _cardDatabase.CreateCard(x)).ToList();

      var decks = new List<List<Card>>();

      // create mono and dual colored decks
      for (int i = 0; i < 5; i++)
      {
        var mono = cards
          .Where(x => ColorFilters[i](x) || x.HasColors(ManaColors.Colorless))
          .ToList();

        var monoDeck = BuildDeckNoLands(mono);
        if (monoDeck != null)
          decks.Add(monoDeck);

        for (int j = i + 1; j < 5; j++)
        {
          var dual = cards
            .Where(x => ColorFilters[i](x) || ColorFilters[j](x) || x.HasColors(ManaColors.Colorless))
            .ToList();
          
          var dualDeck = BuildDeckNoLands(dual);
          
          if (dualDeck != null)
            decks.Add(dualDeck);
        }
      }

      foreach (var deck in decks)
      {
        AddLands(deck);
      }

      return decks.Select(d => d.Select(c => c.Name).ToList()).ToList();
    }

    private void AddLands(List<Card> deck)
    {
      // todo
    }

    private List<Card> BuildDeckNoLands(IEnumerable<Card> cards)
    {
      var creatures = cards
        .Where(x => x.Is().Creature)
        .OrderByDescending(x => _cardRatings.GetRating(x.Name))
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
        .OrderByDescending(x => _cardRatings.GetRating(x.Name))
        .Take(MinSpellCount - MinCreatureCount));

      return result;
    }
  }
}