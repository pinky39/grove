namespace Grove.Gameplay.Decisions
{
  using System.Windows;
  using UserInterface;

  public class SelectStartingPlayer : Decision
  {
    private SelectStartingPlayer() {}

    public SelectStartingPlayer(Player controller)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler()) {}

    private abstract class Handler : DecisionHandler<SelectStartingPlayer, ChosenPlayer>
    {
      public override void ProcessResults()
      {
        Game.Players.Starting = Result.Player;
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new ChosenPlayer(null);
      }

      protected override void ExecuteQuery()
      {
        Result = new ChosenPlayer(D.Controller);
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenPlayer) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = Ui.Shell.ShowMessageBox(
          title: "Decide who will start the game",
          message: "You won the toss, do you want to start the game?",
          buttons: MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
          Result = new ChosenPlayer(D.Controller);
          return;
        }

        Result = new ChosenPlayer(D.Controller.Opponent);
      }
    }
  }
}