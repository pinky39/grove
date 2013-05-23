namespace Grove.UserInterface.Leaderboard
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Caliburn.Micro;
  using Gameplay.Tournaments;
  using Infrastructure;
  using Messages;

  public class ViewModel : IReceive<RoundIsFinished>
  {    
    private readonly BindableCollection<object> _players = new BindableCollection<object>();

    public ViewModel(IEnumerable<TournamentPlayer> players, int roundsLeft, bool canContinue)
    {
      CanNext = canContinue;
      RoundsLeft = roundsLeft;

      _players.AddRange(players.OrderBy(x => x).Select((x, i) => new
        {
          Place = i + 1,
          Player = x
        }));               
    }

    public int RoundsLeft { get; private set; }
    public IEnumerable<object> Players { get { return _players; } }
    public virtual bool CanNext { get; protected set; }
  
    public virtual void Next()
    {            
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<TournamentPlayer> players, int roundsLeft, bool canContinue);
    }

    public void Receive(RoundIsFinished message)
    {
      CanNext = true;
    }
  }
}