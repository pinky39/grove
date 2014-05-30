namespace Grove
{
  using System;
  using Grove.Infrastructure;

  [Serializable, Copyable]
  public class TournamentMatch
  {
    private TournamentMatch() { }

    public TournamentMatch(TournamentPlayer player1, TournamentPlayer player2)
    {
      Player1 = player1;
      Player2 = player2;
    }

    public TournamentPlayer Player1 { get; private set; }
    public TournamentPlayer Player2 { get; private set; }

    public TournamentPlayer HumanPlayer { get { return Player1.IsHuman ? Player1 : Player2; } }
    public TournamentPlayer NonHumanPlayer { get { return Player1.IsHuman ? Player2 : Player1; } }

    public bool IsSimulated { get { return !Player1.IsHuman && !Player2.IsHuman; } }
    public bool IsFinished { get; set; }

    public int Player1WinCount { get; private set; }
    public int Player2WinCount { get; private set; }

    public void SetMatchResults(int player1WinCount, int player2WinCount)
    {
      Player1WinCount = player1WinCount;
      Player2WinCount = player2WinCount;
      
      Player1.GamesWon += Player1WinCount;
      Player2.GamesWon += Player2WinCount;

      Player1.GamesLost += Player2WinCount;
      Player2.GamesLost += Player1WinCount;

      if (Player1WinCount > Player2WinCount)
      {
        Player1.WinCount++;
        Player2.LooseCount++;
      }
      else if (Player1WinCount < Player2WinCount)
      {
        Player2.WinCount++;
        Player1.LooseCount++;
      }
      else
      {
        Player1.DrawCount++;
        Player2.DrawCount++;
      }
    }
  }
}