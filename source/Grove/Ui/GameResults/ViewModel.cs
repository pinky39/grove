namespace Grove.Ui.GameResults
{
  using System;
  using Core;
  using Infrastructure;

  public class ViewModel
  {
    private readonly Match _match;
    private readonly Players _players;

    public ViewModel(Match match, Players players)
    {
      _match = match;
      _players = players;
    }

    public string OpponentsResult
    {
      get
      {
        return string.Format("{0} won {1}",
          _players.Player2,
          GetWinCountText(_match.Player2WinCount));
      }
    }

    public bool PlayerLeftMatch { get; set; }

    public string ResultText
    {
      get
      {
        if (_players.BothHaveLost)
          return "Last game was a draw.";


        if (_players.Player2.HasLost)
          return "Congratulations, you won the game!";

        return "Tough luck, you lost the game!";
      }
    }

    public string WinningAvatar
    {
      get
      {
        return _players.BothHaveLost || _players.Player1.HasLost
          ? _players.Player2.Avatar
          : _players.Player1.Avatar;
      }
    }

    public string YourResult
    {
      get
      {
        return string.Format("{0} won {1}",
          _players.Player1,
          GetWinCountText(_match.Player1WinCount));
      }
    }

    public void LeaveMatch()
    {
      PlayerLeftMatch = true;
      this.Close();
    }

    public void NextGame()
    {
      this.Close();
    }

    private static string GetWinCountText(int winCount)
    {
      if (winCount == 1)
        return "1 game";

      return String.Format("{0} games", winCount);
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}