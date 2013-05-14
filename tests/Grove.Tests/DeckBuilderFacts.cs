namespace Grove.Tests
{
  using System;
  using Artifical;
  using Grove.Infrastructure;
  using Infrastructure;
  using UserInterface;
  using Xunit;

  public class DeckBuilderFacts : Scenario
  {
    [Fact]
    public void BuildDecks()
    {
      EnableLogging("Info");
      
      for (int i = 0; i < 25; i++)
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

        var builder = new DeckBuilder(CardsDatabase, new DeckEvaluator(MatchSimulator));
        var bestDeck = builder.BuildDeck(pileOfCards, set.Ratings);

        Console.WriteLine("Best deck:");
        Console.WriteLine("--------------------------------");

        foreach (var card in bestDeck)
        {
          Console.WriteLine(card);
        }  
      }           
    }
  }
}