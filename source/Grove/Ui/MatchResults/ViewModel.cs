namespace Grove.Ui.MatchResults
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
          _players.Player2.Name,
          GetWinCountText(_match.Player2WinCount));
      }
    }

    public string ResultText
    {
      get
      {
        return _players.Player2.HasLost
                 ? "Congratulations, you won the match!"
                 : "Tough luck, you lost the match!";
      }
    }

    public bool ShouldRematch { get; set; }

    public string WinningAvatar
    {
      get
      {
        return _players.Player1.HasLost
                 ? _players.Player2.Avatar
                 : _players.Player1.Avatar;
      }
    }

    public string YourResult
    {
      get
      {
        return string.Format("{0} won {1}",
          _players.Player1.Name,
          GetWinCountText(_match.Player1WinCount));
      }
    }

    public void Quit()
    {
      this.Close();
    }

    public void Rematch()
    {
      ShouldRematch = true;
      this.Close();
    }

    private static string GetWinCountText(int winCount)
    {
      if (winCount == 1)
        return "1 game";

      return String.Format("{0} games", winCount);
    }

    #region Nested type: IFactory

    public interface IFactory
    {
      ViewModel Create();
    }

    #endregion
  }
}