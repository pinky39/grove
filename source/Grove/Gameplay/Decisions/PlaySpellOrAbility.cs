namespace Grove.Gameplay.Decisions
{
  using System.Collections.Generic;
  using AI;
  using Infrastructure;
  using UserInterface;

  public class PlaySpellOrAbility : Decision
  {
    private PlaySpellOrAbility() {}

    public PlaySpellOrAbility(Player controller)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(), () => new PlaybackHandler()) {}

    private abstract class Handler : DecisionHandler<PlaySpellOrAbility, ChosenPlayable>
    {
      public override bool IsPass { get { return Result.WasPriorityPassed; } }

      public override void ProcessResults()
      {
        Result.Playable.Play();
      }
    }

    private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
    {
      private readonly MachinePlanExecutor _executor;
      private List<IPlayable> _playables;

      public MachineHandler()
      {
        Result = new ChosenPlayable {Playable = new Pass()};
        _executor = new MachinePlanExecutor(this);
      }

      public override bool HasCompleted { get { return _executor.HasCompleted; } }
      bool IMachineExecutionPlan.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

      void IMachineExecutionPlan.ExecuteQuery()
      {
        ExecuteQuery();
      }

      Game ISearchNode.Game { get { return Game; } }
      public Player Controller { get { return D.Controller; } }

      public int ResultCount { get { return _playables.Count; } }

      public void GenerateChoices()
      {
        _playables = GeneratePlayables();

        // consider passing priority every time
        _playables.Add(new Pass());
      }

      public void SetResult(int index)
      {
        Result = new ChosenPlayable {Playable = _playables[index]};
        LogFile.Debug("Move is {0}", _playables[index]);
      }

      public override void Execute()
      {
        _executor.Execute();
      }

      public override string ToString()
      {
        return string.Format("{0}, {1} plays", Game.Turn.Step, Controller);
      }

      protected override void Initialize()
      {
        _executor.Initialize(ChangeTracker);
      }

      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }

      private List<IPlayable> GeneratePlayables()
      {
        if (Stack.TopSpellOwner == Controller || (Ai.IsSearchInProgress && Turn.StepCount > Ai.PlaySpellsUntilDepth))
        {
          // if you own the top spell just pass so it resolves
          // you will get priority again when it resolves
          return new List<IPlayable>();
        }

        return new PlayableGenerator(Controller, Game).GetPlayables();
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenPlayable) Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        Result = ExecuteAssertionsAndGetNextScenarioResult()
          ?? ChosenPlayable.Pass;
      }
    }

    private class UiHandler : Handler
    {            
      protected override void ExecuteQuery()
      {
        if (Ui.Configuration.ShouldAutoPass(
            step:  Game.Turn.Step, 
            isActiveTurn: D.Controller.IsActive, 
            anyPlayerPlayedSomething: Game.Turn.Events.HasSpellsBeenPlayedDuringThisStep))
        {
          Result = new ChosenPlayable {Playable = new Pass()};
          return;
        }

        var dialog = Ui.Dialogs.Priority.Create();
        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.PlaySpellsOrAbilities);
        Result = new ChosenPlayable {Playable = dialog.Playable ?? new Pass()};
      }
    }
  }
}