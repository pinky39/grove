namespace Grove.Tests
{
  using System;
  using Infrastructure;
  using Tournament;
  using UserInterface;

  public class DeckBuilderFacts : Scenario
  {
    //[Fact]
    public void BuildDecks()
    {
      var set = new MagicSet(MediaLibrary.GetSetPath("Urza's Saga"));
      var pileOfCards = set.GenerateMixedPack(boosterCount: 3, tournamentCount: 1);

      Console.WriteLine("Card list:");
      Console.WriteLine("--------------------------------");
      foreach (var card in pileOfCards)
      {
        Console.WriteLine(card);
      }

      Console.WriteLine();

      var builder = new DeckBuilder(CardDatabase);
      var decks = builder.BuildDecks(pileOfCards, set.Ratings);

      for (var i = 0; i < decks.Count; i++)
      {
        var deck = decks[i];

        Console.WriteLine("Deck {0}:", i + 1);
        Console.WriteLine("--------------------------------");

        foreach (var card in deck)
        {
          Console.WriteLine(card);
        }

        Console.WriteLine("\n");
      }

      var evaluator = new DeckEvaluator(MatchSimulator);
      var bestDeck = evaluator.GetBestDeck(decks);

      Console.WriteLine("Best deck:");
      Console.WriteLine("--------------------------------");

      foreach (var card in bestDeck)
      {
        Console.WriteLine(card);
      }
    }
  }
}