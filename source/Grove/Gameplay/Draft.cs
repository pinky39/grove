namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.DraftAlgorithms;
  using Grove.Infrastructure;
  using Grove.UserInterface.DraftScreen;

  public class DraftRunner
  {
    private readonly ViewModel _userInterface;

    public DraftRunner(ViewModel userInterface = null)
    {
      _userInterface = userInterface;
    }

    public DraftResults Run(int playerCount, string[] sets, CardRatings ratings)
    {
      var players = CreatePlayers(playerCount, ratings, _userInterface != null);
      var boosters = CreateBoosters(sets, players);

      var round = 1;
      var direction = 1; // clockwise

      while (round <= 3)
      {
        var roundBoosters = boosters
          .Skip((round - 1)*players.Count)
          .Take(players.Count)
          .ToList();

        var cardCount = roundBoosters[0].Count;

        while (cardCount > 0)
        {
          for (var playerIndex = 0; playerIndex < players.Count; playerIndex++)
          {
            var boosterIndex = (100 + playerIndex + direction*cardCount)%players.Count;
            var player = players[playerIndex];

            var draftedCard = player.Strategy.PickCard(roundBoosters[boosterIndex], round);

            if (_userInterface != null && _userInterface.PlayerLeftDraft)
              return null;

            player.Library.Add(draftedCard);
            roundBoosters[boosterIndex].Remove(draftedCard);
          }

          cardCount = roundBoosters[0].Count;
        }

        round++;
        direction = -direction;
      }

      return new DraftResults(players);
    }

    private static IEnumerable<List<CardInfo>> CreateBoosters(string[] sets, List<DraftPlayer> players)
    {
      var boosters = new List<List<CardInfo>>();

      foreach (var set in sets)
      {
        for (var i = 0; i < players.Count; i++)
        {
          var boosterPack = MediaLibrary.GetSet(set).GenerateBoosterPack();
          boosters.Add(boosterPack);
        }
      }
      return boosters;
    }

    private List<DraftPlayer> CreatePlayers(int playerCount, CardRatings ratings, bool includeHumanPlayer)
    {
      var aiStrategies = new Func<IDraftingStrategy>[]
        {
          () => new Forcing(ratings),
          () => new Greedy(ratings), 
        };

      var players = new List<DraftPlayer>();

      for (var i = 0; i < playerCount; i++)
      {
        players.Add(new DraftPlayer
          {
            Strategy = aiStrategies[RandomEx.Next(10) >= 3 ? 0 : 1]()
          });
      }

      if (includeHumanPlayer)
      {
        players[0] = new DraftPlayer
          {
            Strategy = _userInterface
          };
      }


      return players;
    }
  }
}