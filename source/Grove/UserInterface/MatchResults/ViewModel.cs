namespace Grove.UserInterface.MatchResults
{
  using System;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    public string OpponentsResult
    {
      get
      {
        return string.Format("{0} won {1}",
          Players.Player2,
          GetWinCountText(Match.Player2WinCount));
      }
    }

    public string ResultText
    {
      get
      {
        return Players.Player2.HasLost
          ? "Congratulations, you won the match!"
          : "Tough luck, you lost the match!";
      }
    }

    public bool ShouldRematch { get; set; }

    public string WinningAvatar
    {
      get
      {
        return Players.Player1.HasLost
          ? Players.Player2.Avatar
          : Players.Player1.Avatar;
      }
    }

    public string YourResult
    {
      get
      {
        return string.Format("{0} won {1}",
          Players.Player1,
          GetWinCountText(Match.Player1WinCount));
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