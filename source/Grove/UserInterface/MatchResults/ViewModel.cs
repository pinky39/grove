namespace Grove.UserInterface.MatchResults
{
  using System;
  using Gameplay;
  using Infrastructure;

  public class ViewModel
  {
    private readonly Game _game;
    private readonly Match _match;


    public ViewModel(Match match, Game game)
    {
      _match = match;
      _game = game;
    }

    public string OpponentsResult
    {
      get
      {
        return string.Format("{0} won {1}",
          _game.Players.Player2,
          GetWinCountText(_match.Player2WinCount));
      }
    }

    public string ResultText
    {
      get
      {
        return _game.Players.Player2.HasLost
          ? "Congratulations, you won the match!"
          : "Tough luck, you lost the match!";
      }
    }

    public bool ShouldRematch { get; set; }

    public string WinningAvatar
    {
      get
      {
        return _game.Players.Player1.HasLost
          ? _game.Players.Player2.Avatar
          : _game.Players.Player1.Avatar;
      }
    }

    public string YourResult
    {
      get
      {
        return string.Format("{0} won {1}",
          _game.Players.Player1,
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

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}