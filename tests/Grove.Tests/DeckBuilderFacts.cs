namespace Grove.Tests
{
  using System;
  using Artifical;
  using Grove.Infrastructure;
  using Infrastructure;
  using UserInterface;

  public class DeckBuilderFacts : Scenario
  {
    public DeckBuilderFacts()
    {
      MediaLibrary.LoadSets();
    }

    //[Fact]
    public void BuildDecks()
    {
      for (var i = 0; i < 1; i++)
      {
        LogFile.Info("Building deck {0} of 25...", i + 1);

        var set = MediaLibrary.GetSet("Urza's Saga");
        var pileOfCards = set.GenerateMixedPack(boosterCount: 3, tournamentCount: 1);

        Console.WriteLine("Card list:");
        Console.WriteLine("--------------------------------");
        foreach (var card in pileOfCards)
        {
          Console.WriteLine(card);
        }

        Console.WriteLine();

        var builder = new DeckBuilder(CardDatabase, new DeckEvaluator(MatchSimulator));
        var bestDeck = builder.BuildDeck(pileOfCards, set.Ratings);

        Console.WriteLine("Best deck:");
        Console.WriteLine("--------------------------------");


        Console.WriteLine(bestDeck);        
      }
    }
  }
}