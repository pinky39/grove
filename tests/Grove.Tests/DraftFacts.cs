namespace Grove.Tests
{
  using System;
  using System.Linq;
  using Gameplay;
  using Gameplay.AI;
  using Infrastructure;

  public class DraftFacts : Scenario
  {
    //[Fact]

    public DraftFacts()
    {
      MediaLibrary.LoadSets();
    }

    public void DraftLibraries()
    {
      var sets = new[] {"Urza's Saga", "Urza's Saga", "Urza's Saga"};
      var ratings = MediaLibrary.GetSet(sets[0]).Ratings;

      var runner = new DraftRunner();

      var results = runner.Run(
        playerCount: 8,
        sets: sets,
        ratings: ratings);

      Equal(8, results.Libraries.Count);
      True(results.Libraries.All(x => x.Count == 45));

      for (var i = 0; i < 8; i++)
      {
        Console.WriteLine("Player {0}", i + 1);
        Console.WriteLine("-------------------");

        var library = results.Libraries[i];

        Console.WriteLine("White cards: {0}", library.Count(x => Gameplay.Cards.All[x.Name].HasColor(CardColor.White)));
        Console.WriteLine("Blue cards: {0}", library.Count(x => Gameplay.Cards.All[x.Name].HasColor(CardColor.Blue)));
        Console.WriteLine("Black cards: {0}", library.Count(x => Gameplay.Cards.All[x.Name].HasColor(CardColor.Black)));
        Console.WriteLine("Red cards: {0}", library.Count(x => Gameplay.Cards.All[x.Name].HasColor(CardColor.Red)));
        Console.WriteLine("Green cards: {0}", library.Count(x => Gameplay.Cards.All[x.Name].HasColor(CardColor.Green)));
        Console.WriteLine("Colorless cards: {0}",
          library.Count(x => Gameplay.Cards.All[x.Name].HasColor(CardColor.Colorless)));

        var deck = DeckBuilder.BuildDeck(library, ratings);
        Console.WriteLine();

        Console.WriteLine("Creatures: {0}, Spells {1}",
          deck.Count(x => Gameplay.Cards.All[x.Name].Is().Creature),
          deck.Count(x => !Gameplay.Cards.All[x.Name].Is().Creature && !Gameplay.Cards.All[x.Name].Is().Land));

        Console.WriteLine("-------------------");
        Console.WriteLine(deck);

        Console.WriteLine();
      }
    }
  }
}