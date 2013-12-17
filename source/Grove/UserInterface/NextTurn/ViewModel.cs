namespace Grove.UserInterface.NextTurn
{
  using System;

  public class ViewModel : ViewModelBase
  {
    public int Avatar { get { return CurrentGame.Players.Active.AvatarId; } }
    public int TurnNumber { get { return CurrentGame.Turn.TurnCount;  } }

    public string Message
    {
      get
      {
        var playerName = CurrentGame.Players.Active.Name;

        playerName = playerName.Equals("you", StringComparison.CurrentCultureIgnoreCase)
          ? "your" : playerName + "'s";

        return String.Format("It's {0} turn.", playerName);
      }
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}