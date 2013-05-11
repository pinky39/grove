namespace Grove.Utils
{
  using System;
  using Artifical;
  using UserInterface;

  public class GenerateDeck : Task
  {
    private readonly DeckBuilder _deckBuilder;

    public GenerateDeck(DeckBuilder deckBuilder)
    {
      _deckBuilder = deckBuilder;
    }

    public override void Execute(Arguments arguments)
    {
      var set = MediaLibrary.GetSet(arguments["set"]);
      var cards = set.GenerateMixedPack(boosterCount: 3, tournamentCount: 1);

      Console.WriteLine("Card list:");
      Console.WriteLine("--------------------------------");
      foreach (var card in cards)
      {
        Console.WriteLine(card);
      }

      Console.WriteLine();

      var bestDeck = _deckBuilder.BuildDeck(cards, set.Ratings);


      Console.WriteLine("Best deck:");
      Console.WriteLine("--------------------------------");

      foreach (var card in bestDeck)
      {
        Console.WriteLine(card);
      }
    }
  }
}