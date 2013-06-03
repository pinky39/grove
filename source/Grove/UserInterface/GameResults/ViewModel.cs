namespace Grove.UserInterface.GameResults
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
          GetWinCountText(CurrentMatch.Player2WinCount));
      }
    }

    public bool PlayerLeftMatch { get; set; }

    public string ResultText
    {
      get
      {
        if (Players.BothHaveLost)
          return "Last game was a draw.";


        if (Players.Player2.HasLost)
          return "Congratulations, you won the game!";

        return "Tough luck, you lost the game!";
      }
    }

    public string WinningAvatar
    {
      get
      {
        return Players.BothHaveLost || Players.Player1.HasLost
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
          GetWinCountText(CurrentMatch.Player1WinCount));
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