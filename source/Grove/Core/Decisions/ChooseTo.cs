namespace Grove.Decisions
{
  using System;
  using System.Windows;
  using Grove.UserInterface;

  public class ChooseTo : Decision
  {
    private readonly Params _p = new Params();

    private ChooseTo() {}

    public ChooseTo(Player controller, Action<Params> setParameters)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<ChooseTo, BooleanResult>
    {
      public override void ProcessResults()
      {
        D._p.ProcessDecisionResults.ProcessResults(Result);
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = false;
      }

      protected override void ExecuteQuery()
      {
        Result = D._p.ChooseDecisionResults.ChooseResult();
      }
    }

    public class Params
    {
      public IChooseDecisionResults<BooleanResult> ChooseDecisionResults;
      public IProcessDecisionResults<BooleanResult> ProcessDecisionResults;
      public string Text;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (BooleanResult) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = Ui.Shell.ShowMessageBox(
          message: D._p.Text,
          type: DialogType.Small,
          buttons: MessageBoxButton.YesNo);

        Result = result == MessageBoxResult.Yes;
      }
    }
  }
}