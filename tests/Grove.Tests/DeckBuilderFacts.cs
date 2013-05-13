namespace Grove.Tests
{
  using System;
  using Artifical;
  using Gameplay.Sets;
  using Infrastructure;

  public class DeckBuilderFacts : Scenario
  {
    //[Fact]
    public void BuildDecks()
    {
      var set = new MagicSet("Urza's Saga");
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