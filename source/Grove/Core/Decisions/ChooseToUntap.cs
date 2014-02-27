namespace Grove.Decisions
{
  using System.Windows;
  using Grove.AI;
  using Grove.UserInterface;

  public class ChooseToUntap : Decision
  {
    private readonly Card _card;

    private ChooseToUntap() {}

    public ChooseToUntap(Player controller, Card card)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(), () => new PlaybackHandler())
    {
      _card = card;
    }

    private abstract class Handler : DecisionHandler<ChooseToUntap, BooleanResult>
    {
      protected override bool ShouldExecuteQuery { get { return D._card.IsTapped; } }

      public override void ProcessResults()
      {
        if (Result.IsTrue)
        {
          D._card.Untap();
        }
      }

      protected override void SetResultNoQuery()
      {
        Result = false;
      }
    }

    private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
    {
      private readonly MachinePlanExecutor _executor;

      public MachineHandler()
      {
        _executor = new MachinePlanExecutor(this);
        Result = false;
      }

      public override bool HasCompleted { get { return _executor.HasCompleted; } }
      bool IMachineExecutionPlan.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

      void IMachineExecutionPlan.ExecuteQuery()
      {
        ExecuteQuery();
      }

      Game ISearchNode.Game { get { return Game; } }
      public Player Controller { get { return D.Controller; } }

      public int ResultCount { get { return 2; } }

      public void SetResult(int index)
      {
        Result = index != 0;
      }

      public void GenerateChoices() {}

      public override void Execute()
      {
        _executor.Execute();
      }

      protected override void Initialize()
      {
        _executor.Initialize(ChangeTracker);
      }


      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }
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

    private class ScenarioHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        Result = ExecuteAssertionsAndGetNextScenarioResult()
          ?? false;
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = Ui.Shell.ShowMessageBox(
          message: string.Format("Untap {0}?", D._card),
          buttons: MessageBoxButton.YesNo,
          type: DialogType.Small);

        Result = result == MessageBoxResult.Yes;
      }
    }
  }
}