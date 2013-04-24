namespace Grove.Utils
{
  using System;
  using Core;
  using Tournament;
  using Ui;

  public class GenerateDeck : Task
  {
    private readonly DeckBuilder _deckBuilder;
    private readonly DeckEvaluator _deckEvaluator;

    public GenerateDeck(DeckBuilder deckBuilder, DeckEvaluator deckEvaluator)
    {
      _deckBuilder = deckBuilder;
      _deckEvaluator = deckEvaluator;
    }

    public override void Execute(Arguments arguments)
    {
      var set = new MagicSet(MediaLibrary.GetSetPath(arguments["set"]));
      var cards = set.GenerateMixedPack(boosterCount: 3, tournamentCount: 1);

      Console.WriteLine("Card list:");
      Console.WriteLine("--------------------------------");
      foreach (var card in cards)
      {
        Console.WriteLine(card);
      }

      Console.WriteLine();

      var decks = _deckBuilder.BuildDecks(cards, set.Ratings);

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


      var bestDeck = _deckEvaluator.GetBestDeck(decks);

      Console.WriteLine("Best deck:");
      Console.WriteLine("--------------------------------");

      foreach (var card in bestDeck)
      {
        Console.WriteLine(card);
      }
    }
  }
}