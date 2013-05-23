namespace Grove.UserInterface.Leaderboard
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Tournaments;
  using Infrastructure;

  public class ViewModel
  {
    private readonly List<object> _players = new List<object>();

    public ViewModel(IEnumerable<TournamentPlayer> players, int roundsLeft)
    {
      RoundsLeft = roundsLeft;

      _players.AddRange(players.OrderBy(x => x).Select((x, i) => new
        {
          Place = i + 1,
          Player = x
        }));
    }

    public int RoundsLeft { get; private set; }
    public IEnumerable<object> Players { get { return _players; } }

    public virtual void Next()
    {
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> players, int roundsLeft);
    }
  }
}