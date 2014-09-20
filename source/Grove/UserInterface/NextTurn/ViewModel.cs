namespace Grove.UserInterface.NextTurn
{
  using System;

  public class ViewModel : ViewModelBase
  {
    public int Avatar { get { return Game.Players.Active.AvatarId; } }
    public int TurnNumber { get { return Game.Turn.TurnCount;  } }

    public string Message
    {
      get
      {
        var playerName = Game.Players.Active.Name;

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