namespace Grove.Core.Controllers.Human
{
  using System.Windows;
  using Ui.Shell;

  public class SelectStartingPlayer : Controllers.SelectStartingPlayer
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        title: "Decide who will start the game",
        message: "You won the toss, do you want to start the game?",
        buttons: MessageBoxButton.YesNo);

      if (result == MessageBoxResult.Yes)
      {
        Result = Controller;
        return;
      }

      Result = Game.Players.GetOpponent(Controller);
    }
  }
}