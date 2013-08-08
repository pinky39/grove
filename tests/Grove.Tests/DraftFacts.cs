namespace Grove.Tests
{
  using System;
  using System.Linq;
  using Artifical;
  using Artifical.DraftAlgorithms;
  using Gameplay.Characteristics;
  using Gameplay.Tournaments;
  using Infrastructure;
  using UserInterface;
  using Xunit;

  public class DraftFacts : Scenario
  {
    //[Fact]
    public void DraftLibraries()
    {
      var sets = new[] {"Urza's Saga", "Urza's Saga", "Urza's Saga"};
      var ratings = MediaLibrary.GetSet(sets[0]).Ratings;

      var runner = new DraftRunner(DraftStrategies);

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

        Console.WriteLine("White cards: {0}", library.Count(x => CardDatabase[x.Name].HasColor(CardColor.White)));
        Console.WriteLine("Blue cards: {0}", library.Count(x => CardDatabase[x.Name].HasColor(CardColor.Blue)));
        Console.WriteLine("Black cards: {0}", library.Count(x => CardDatabase[x.Name].HasColor(CardColor.Black)));
        Console.WriteLine("Red cards: {0}", library.Count(x => CardDatabase[x.Name].HasColor(CardColor.Red)));
        Console.WriteLine("Green cards: {0}", library.Count(x => CardDatabase[x.Name].HasColor(CardColor.Green)));
        Console.WriteLine("Colorless cards: {0}",
          library.Count(x => CardDatabase[x.Name].HasColor(CardColor.Colorless)));

        var deck = DeckBuilder.BuildDeck(library, ratings);
        Console.WriteLine();

        Console.WriteLine("Creatures: {0}, Spells {1}",
          deck.Count(x => CardDatabase[x.Name].Is().Creature),
          deck.Count(x => !CardDatabase[x.Name].Is().Creature && !CardDatabase[x.Name].Is().Land));

        Console.WriteLine("-------------------");
        Console.WriteLine(deck);

        Console.WriteLine();
      }
    }

    public DraftFacts()
    {
      MediaLibrary.LoadSets();
    }

    private IDraftingStrategies DraftStrategies { get { return Container.Resolve<IDraftingStrategies>(); } }
    private DeckBuilder DeckBuilder { get { return Container.Resolve<DeckBuilder>(); } }
  }
}