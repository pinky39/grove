namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Gameplay;
  using Gameplay.Sets;
  using Persistance;
  using UserInterface;

  public class GenerateDecks : Task
  {
    private readonly DeckBuilder _deckBuilder;    

    public GenerateDecks(DeckBuilder deckBuilder)
    {
      _deckBuilder = deckBuilder;      
    }

    public override void Execute(Arguments arguments)
    {
      var count = int.Parse(arguments["count"]);
      var starterSet = MediaLibrary.GetSet(arguments["starter"]);
      var boosterSet1 = MediaLibrary.GetSet(arguments["booster1"]);
      var boosterSet2 = MediaLibrary.GetSet(arguments["booster2"]);
      var boosterSet3 = MediaLibrary.GetSet(arguments["booster3"]);

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

        DeckFile.Write(deck, Guid.NewGuid() +".dec");
      }
    }
  }
}