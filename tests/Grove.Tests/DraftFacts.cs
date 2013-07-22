namespace Grove.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Artifical;
  using Gameplay.Characteristics;
  using Gameplay.Tournaments;
  using Infrastructure;
  using UserInterface;
  using Xunit;

  public class DraftFacts : Scenario
  {            
    private DraftCardPicker DraftCardPicker { get { return Container.Resolve<DraftCardPicker>(); } }
    private DeckBuilder DeckBuilder { get { return Container.Resolve<DeckBuilder>(); } }

    public DraftFacts()
    {
      MediaLibrary.LoadSets();
    }
    
    [Fact]
    public void DraftLibraries()
    {
      var draft = new Draft(
        machinePicker: DraftCardPicker,
        humanPicker: null);

      var players = new List<TournamentPlayer>();

      for (int i = 0; i < 8; i++)
      {
        players.Add(new TournamentPlayer("Player" + i, isHuman: false));
      }

      var sets = new[] {"Urza's Saga", "Urza's Saga", "Urza's Saga"};
      var ratings = MediaLibrary.GetSet(sets[0]).Ratings;

      var libraries = draft.Run(players, sets, ratings);

      Equal(8, libraries.Count);
      True(libraries.All(x => x.Count == 45));

      for (int i = 0; i < players.Count; i++)
      {
        Console.WriteLine("Player {0}", i + 1);
        Console.WriteLine("-------------------");

        var library = libraries[i];
        
        Console.WriteLine("White cards: {0}", library.Count(x => CardsDictionary[x.Name].HasColor(CardColor.White)));
        Console.WriteLine("Blue cards: {0}", library.Count(x => CardsDictionary[x.Name].HasColor(CardColor.Blue)));
        Console.WriteLine("Black cards: {0}", library.Count(x => CardsDictionary[x.Name].HasColor(CardColor.Black)));
        Console.WriteLine("Red cards: {0}", library.Count(x => CardsDictionary[x.Name].HasColor(CardColor.Red)));
        Console.WriteLine("Green cards: {0}", library.Count(x => CardsDictionary[x.Name].HasColor(CardColor.Green)));
        Console.WriteLine("Colorless cards: {0}", library.Count(x => CardsDictionary[x.Name].HasColor(CardColor.Colorless)));

        var deck = DeckBuilder.BuildDeck(library, ratings);
        Console.WriteLine();
        
        Console.WriteLine("Creatures: {0}, Spells {1}", 
            deck.Count(x => CardsDictionary[x.Name].Is().Creature), 
            deck.Count(x => !CardsDictionary[x.Name].Is().Creature && !CardsDictionary[x.Name].Is().Land));
        
        Console.WriteLine("-------------------");        
        Console.WriteLine(deck);

        Console.WriteLine();
      }    
    }
  }
}