namespace Grove.Gameplay.Tournaments
{
  using System.Collections.Generic;
  using System.Linq;
  using Artifical;
  using UserInterface;

  public class Draft
  {
    private readonly IDraftCardPicker _humanPicker;
    private readonly IDraftCardPicker _machinePicker;

    public Draft(IDraftCardPicker machinePicker, IDraftCardPicker humanPicker)
    {
      _machinePicker = machinePicker;
      _humanPicker = humanPicker;
    }

    public List<List<CardInfo>> Run(List<TournamentPlayer> players, string[] sets, CardRatings ratings)
    {
      var libraries = new List<List<CardInfo>>();

      foreach (var player in players)
      {
        libraries.Add(new List<CardInfo>());
      }

      var boosters = new List<List<CardInfo>>();

      foreach (var set in sets)
      {
        for (var i = 0; i < players.Count; i++)
        {
          var boosterPack = MediaLibrary.GetSet(set).GenerateBoosterPack();
          boosters.Add(boosterPack);
        }
      }

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

            var draftedCard = player.IsHuman
              ? _humanPicker.PickCard(libraries[playerIndex], roundBoosters[boosterIndex], round, ratings)
              : _machinePicker.PickCard(libraries[playerIndex], roundBoosters[boosterIndex], round, ratings);

            libraries[playerIndex].Add(draftedCard);
            roundBoosters[boosterIndex].Remove(draftedCard);
          }

          cardCount = roundBoosters[0].Count;
        }

        round++;
        direction = -direction;
      }

      return libraries;
    }
  }
}