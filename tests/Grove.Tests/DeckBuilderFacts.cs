namespace Grove.Tests
{
  using System;
  using AI;
  using Infrastructure;
  using Media;

  public class DeckBuilderFacts : Scenario
  {
    //[Fact]

    public DeckBuilderFacts()
    {
      MediaLibrary.LoadSets();
    }

    public void BuildDecks()
    {
      for (var i = 0; i < 1; i++)
      {
        var set = MediaLibrary.GetSet("Urza's Saga");
        var pileOfCards = set.GenerateMixedPack(boosterCount: 3, tournamentCount: 1);

        Console.WriteLine("Card list:");
        Console.WriteLine("--------------------------------");
        foreach (var card in pileOfCards)
        {
          Console.WriteLine(card);
        }

        Console.WriteLine();

        var bestDeck = DeckBuilder.BuildDeck(pileOfCards, set.Ratings);

        Console.WriteLine("Best deck:");
        Console.WriteLine("--------------------------------");


        Console.WriteLine(bestDeck);
      }
    }
  }
}