namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Artifical;
  using UserInterface;

  public class Tournament
  {
    private readonly DeckBuilder _deckBuilder;
    private readonly List<TournamentPlayer> _players = new List<TournamentPlayer>();
    private CardRatings _cardRatings;
    private TournamentState _state;

    public Tournament(DeckBuilder deckBuilder)
    {
      _deckBuilder = deckBuilder;
    }


    private TournamentPlayer HumanPlayer { get { return _players[0]; } }
    private IEnumerable<TournamentPlayer> NonHumanPlayers { get { return _players.Skip(1); } }

    public void Start(string playerName, int playersCount, string[] boosterPacks, string tournamentPack)
    {
      var roundsToGo = GetRoundCount(playersCount);

      CreatePlayers(playersCount, playerName, tournamentPack, boosterPacks);
      LoadCardRatings(tournamentPack, boosterPacks);

      GenerateDecks();
      ShowEditDeckScreen();

      while (roundsToGo > 0)
      {
        PlayNextRound();
        ShowResults();

        roundsToGo--;
      }
    }

    private void ShowResults() {}

    private int GetRoundCount(int playersCount)
    {
      if (playersCount <= 8)
        return 3;
      if (playersCount <= 16)
        return 4;
      if (playersCount <= 32)
        return 5;
      if (playersCount <= 32)
        return 5;
      if (playersCount <= 64)
        return 6;
      if (playersCount <= 128)
        return 7;
      if (playersCount <= 226)
        return 8;
      if (playersCount <= 409)
        return 9;

      return 10;
    }

    private void PlayNextRound() {}

    private void ShowEditDeckScreen() {}

    private void LoadCardRatings(string tournamentPack, string[] boosterPacks)
    {
      CardRatings merged = null;

      foreach (var setName in boosterPacks)
      {
        var ratings = MediaLibrary.GetSet(setName).Ratings;
        if (merged == null)
        {
          merged = ratings;
        }
        else
        {
          merged = CardRatings.Merge(merged, ratings);
        }
      }

      _cardRatings = CardRatings.Merge(merged, MediaLibrary.GetSet(tournamentPack).Ratings);
    }


    private void GenerateDecks()
    {
      _state = TournamentState.GeneratingDecks;
      var tasks = new List<Task>();

      foreach (var player in NonHumanPlayers)
      {
        var p = player;
        var task = Task.Factory.StartNew(() => p.GenerateDeck(_deckBuilder, _cardRatings));
        tasks.Add(task);
      }

      Task.Factory.ContinueWhenAll(tasks.ToArray(), delegate
        {
          _state = TournamentState.Ready;
        });            
    }

    private void CreatePlayers(int playersCount, string playerName, string tournamentPack, string[] boosterPacks)
    {
      var names = MediaLibrary.NameGenerator.GenerateNames(playersCount - 1);
      _players.Add(new TournamentPlayer(playerName, GenerateLibrary(tournamentPack, boosterPacks)));

      for (var i = 0; i < playersCount - 1; i++)
      {
        _players.Add(new TournamentPlayer(names[i], GenerateLibrary(tournamentPack, boosterPacks)));
      }
    }

    private List<string> GenerateLibrary(string tournamentPack, string[] boosterPacks)
    {
      var library = new List<string>();

      foreach (var setName in boosterPacks)
      {
        library.AddRange(MediaLibrary
          .GetSet(setName)
          .GenerateBoosterPack());
      }

      library.AddRange(MediaLibrary
        .GetSet(tournamentPack)
        .GenerateTournamentPack());

      return library;
    }
  }
}