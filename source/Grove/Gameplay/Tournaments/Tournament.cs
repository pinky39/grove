namespace Grove.Gameplay.Tournaments
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Artifical;
  using Infrastructure;
  using Sets;
  using UserInterface;
  using UserInterface.Messages;
  using UserInterface.Shell;

  public class Tournament
  {
    private readonly DeckBuilder _deckBuilder;
    private readonly List<TournamentPlayer> _players = new List<TournamentPlayer>();
    private readonly PreConstructedLimitedDecks _preConstructedLimitedDecks;
    private readonly IShell _shell;
    private readonly ViewModelFactories _viewModels;
    private CardRatings _cardRatings;

    public Tournament(DeckBuilder deckBuilder, ViewModelFactories viewModels,
      PreConstructedLimitedDecks preConstructedLimitedDecks, IShell shell)
    {
      _deckBuilder = deckBuilder;
      _viewModels = viewModels;
      _preConstructedLimitedDecks = preConstructedLimitedDecks;
      _shell = shell;
    }

    private TournamentPlayer HumanPlayer { get { return _players[0]; } }
    private IEnumerable<TournamentPlayer> NonHumanPlayers { get { return _players.Skip(1); } }

    public void Start(string playerName, int playersCount, string[] boosterPacks, string tournamentPack)
    {
      var roundsToGo = GetRoundCount(playersCount);

      CreatePlayers(playersCount, playerName);
      LoadCardRatings(tournamentPack, boosterPacks);

      GenerateDecks(tournamentPack, boosterPacks);
      ShowEditDeckScreen(GenerateLibrary(tournamentPack, boosterPacks));

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

    private void ShowEditDeckScreen(IEnumerable<string> library)
    {
      var screen = _viewModels.BuildLimitedDeck.Create(library);
      _shell.ChangeScreen(screen, blockUntilClosed: true);
    }

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

    private void GenerateDecks(string tournamentPack, string[] boosterPacks)
    {
      const int minNumberOfGeneratedDecks = 10;
      var limitedCode = MagicSet.GetLimitedCode(tournamentPack, boosterPacks);

      var preconstructed = _preConstructedLimitedDecks.GetDecks(limitedCode)
        .Shuffle()
        .Take(NonHumanPlayers.Count() - minNumberOfGeneratedDecks)
        .ToList();

      var nonHumanPlayers = NonHumanPlayers.ToList();
      var actualNumberOfGeneratedDecks = NonHumanPlayers.Count() - preconstructed.Count;

      for (var i = 0; i < preconstructed.Count; i++)
      {
        nonHumanPlayers[i].Deck = preconstructed[i].ToList();
      }

      Task.Factory.StartNew(() =>
        {
          var count = 0;

          for (var i = preconstructed.Count; i < nonHumanPlayers.Count; i++)
          {
            var player = nonHumanPlayers[i];
            var library = GenerateLibrary(tournamentPack, boosterPacks);
            player.Deck = _deckBuilder.BuildDeck(library, _cardRatings);

            count++;

            _shell.Publish(new DeckGenerated
              {
                Count = count,
                TotalCount = actualNumberOfGeneratedDecks
              });
          }
        });
    }

    private void CreatePlayers(int playersCount, string playerName)
    {
      var names = MediaLibrary.NameGenerator.GenerateNames(playersCount - 1);
      _players.Add(new TournamentPlayer(playerName));

      for (var i = 0; i < playersCount - 1; i++)
      {
        _players.Add(new TournamentPlayer(names[i]));
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