namespace Grove.UserInterface.Leaderboard
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Tournaments;
  using Infrastructure;
  using Messages;
  using Persistance;

  public class ViewModel : ViewModelBase, IReceive<TournamentMatchFinished>
  {
    private readonly List<TournamentPlayer> _finishedPlayers =
      new List<TournamentPlayer>();

    private readonly int _playerCount;

    public ViewModel(IEnumerable<TournamentPlayer> players, int roundsLeft)
    {
      RoundsLeft = roundsLeft;
      var matchesPlayed = players.Max(x => x.MatchesPlayed);
      _playerCount = players.Count();

      var finished = players
        .Where(x => x.MatchesPlayed == matchesPlayed)
        .ToList();

      _finishedPlayers = new List<TournamentPlayer>(finished);
      _finishedPlayers.Sort();
    }

    public int RoundsLeft { get; private set; }
    public int MatchesInProgress { get { return (_playerCount - _finishedPlayers.Count)/2; } }

    public IEnumerable<object> FinishedPlayers
    {
      get
      {
        return _finishedPlayers.Select((x, i) => new
          {
            Place = i + 1,
            IsOdd = (i + 1)%2 != 0,
            Player = x
          });
      }
    }

    public bool CanContinue { get { return MatchesInProgress == 0; } }

    [Updates("Players", "MatchesInProgress", "CanContinue")]
    public virtual void Receive(TournamentMatchFinished message)
    {
      _finishedPlayers.Add(message.Match.Player1);
      _finishedPlayers.Add(message.Match.Player2);

      _finishedPlayers.Sort();
    }

    public virtual void Continue()
    {
      this.Close();
    }

    public void Save()
    {
      var saveFileHeader = new SaveFileHeader
        {
          Description = CurrentTournament.Description
        };

      var savedTournament = CurrentTournament.Save();
      SaveLoadHelper.WriteToDisk(saveFileHeader, savedTournament);
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> players, int roundsLeft);
    }
  }
}