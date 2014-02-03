namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Artifical;
  using Gameplay;
  using Gameplay.Sets;
  using Persistance;
  using UserInterface;

  public class GenerateDecks : Task
  {
    private readonly DeckBuilder _deckBuilder;    

    public GenerateDecks(DeckBuilder deckBuilder, CardDatabase cardDatabase, CardFactory cardFactory)
    {
      _deckBuilder = deckBuilder;     
      cardDatabase.Initialize(cardFactory.CreateAll());       
    }

    public override bool Execute(Arguments arguments)
    {            
      if (arguments.Count != 1 && arguments.Count != 5)
        return false;
      
      var count = int.Parse(arguments["count"]);

      MagicSet starterSet;
      MagicSet boosterSet1;
      MagicSet boosterSet2;
      MagicSet boosterSet3;
      
      if (arguments.Count == 1) 
      {
        starterSet = MediaLibrary.RandomSet();
        boosterSet1 = MediaLibrary.RandomSet();
        boosterSet2 = MediaLibrary.RandomSet();
        boosterSet3 = MediaLibrary.RandomSet();
      }
      else
      {
        starterSet = MediaLibrary.GetSet(arguments["s"]);
        boosterSet1 = MediaLibrary.GetSet(arguments["b1"]);
        boosterSet2 = MediaLibrary.GetSet(arguments["b2"]);
        boosterSet3 = MediaLibrary.GetSet(arguments["b3"]);
      }

      Console.WriteLine("Starter: {0}\nBooster1: {1}\nBooster2: {2}\nBooster3: {3}\n", 
        starterSet.Name, boosterSet1.Name, boosterSet2.Name, boosterSet3.Name);

      for (var i = 0; i < count; i++)
      {
        var starter = starterSet.GenerateTournamentPack();
        var booster1 = boosterSet1.GenerateBoosterPack();
        var booster2 = boosterSet2.GenerateBoosterPack();
        var booster3 = boosterSet3.GenerateBoosterPack();

        var library = new List<CardInfo>();
        
        library.AddRange(starter);
        library.AddRange(booster1);
        library.AddRange(booster2);
        library.AddRange(booster3);

        var ratings = CardRatings.Merge(CardRatings.Merge(boosterSet2.Ratings, boosterSet3.Ratings), 
          CardRatings.Merge(starterSet.Ratings, boosterSet1.Ratings));

        var deck = _deckBuilder.BuildDeck(library, ratings);        
        deck.LimitedCode = MagicSet.GetLimitedCode(starterSet.Name, new[]{boosterSet1.Name, boosterSet2.Name, boosterSet3.Name});

        File.WriteAllBytes(Guid.NewGuid() + ".dec", DeckFile.Write(deck));
      }

      return true;
    }

    public override void Usage()
    {
     Console.WriteLine(
       "usage:ugrove gen count=1000 [s=set b1=set b2=set b3=set]\n\nGenerates 1000 decks for sealed tournaments taking random \nor specified tournament and booster packs.");
    }
  }
}