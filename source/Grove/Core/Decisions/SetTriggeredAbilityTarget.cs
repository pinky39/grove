namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.Infrastructure;
  using Grove.UserInterface;
  using Grove.UserInterface.SelectTarget;

  public class SetTriggeredAbilityTarget : Decision
  {
    private readonly Params _p = new Params();

    private SetTriggeredAbilityTarget()
    {
    }

    public SetTriggeredAbilityTarget(Player controller, Action<Params> setParameters) : base(controller,
      () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<SetTriggeredAbilityTarget, ChosenTargets>
    {
      public override void ProcessResults()
      {
        var effectParameters = new EffectParameters
          {
            Source = D._p.Source,
            Targets = Result.Targets,
            TriggerMessage = D._p.TriggerMessage
          };

        var effect = D._p.EffectFactory();
        if (Result.HasTargets == false)
        {
          effect.Initialize(effectParameters, Game, evaluateDynamicParameters: false);
          effect.EffectCountered(SpellCounterReason.IllegalTarget);
          return;
        }

        effect.Initialize(effectParameters, Game);
        Stack.Push(effect);
      }
    }

    private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
    {
      private readonly MachinePlanExecutor _executor;
      private List<Targets> _targets;

      public MachineHandler()
      {
        Result = new ChosenTargets(null);
        _executor = new MachinePlanExecutor(this);
      }

      public override bool HasCompleted
      {
        get { return _executor.HasCompleted; }
      }

      public bool IsMax
      {
        get { return Controller.IsMax; }
      }

      bool IMachineExecutionPlan.ShouldExecuteQuery
      {
        get { return ShouldExecuteQuery; }
      }

      void IMachineExecutionPlan.ExecuteQuery()
      {
        ExecuteQuery();
      }

      Game ISearchNode.Game
      {
        get { return Game; }
      }

      public Player Controller
      {
        get { return D.Controller; }
      }

      public int ResultCount
      {
        get { return _targets.Count; }
      }

      public void GenerateChoices()
      {
        _targets = GenerateTargets().ToList();
      }

      public void SetResult(int index)
      {
        Result = new ChosenTargets(_targets[index]);
      }

      public override void Execute()
      {
        _executor.Execute();
      }

      public override string ToString()
      {
        return string.Format("{0}: {1} sets trig. ability targets", Game.Turn.Step, Controller);
      }

      protected override void Initialize()
      {
        _executor.Initialize(ChangeTracker);
      }

      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }

      private IEnumerable<Targets> GenerateTargets()
      {
        var targetsCandidates = TargetingHelper.GenerateTargets(
          D._p.Source.OwningCard,
          D._p.TargetSelector,
          D._p.MachineRules.Where(x => x is TargetingRule).Cast<TargetingRule>(),
          force: true,
          triggerMessage: D._p.TriggerMessage);

        return targetsCandidates.Take(Ai.Parameters.TargetCount);
      }
    }

    [Copyable]
    public class Params
    {
      public EffectFactory EffectFactory;
      public List<MachinePlayRule> MachineRules;
      public TriggeredAbility Source;
      public TargetSelector TargetSelector;
      public object TriggerMessage;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery
      {
        get { return true; }
      }

      public override void SaveDecisionResults()
      {
      }

      protected override void ExecuteQuery()
      {
        Result = (ChosenTargets) Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        Result = ExecuteAssertionsAndGetNextScenarioResult() ?? ChosenTargets.None();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var targets = new Targets();

        foreach (var validator in D._p.TargetSelector.Effect)
        {
          if (NoValidTargets(validator))
            continue;

          var selectTargetParameters = new SelectTargetParameters
            {
              Validator = validator,
              CanCancel = false,
              TriggerMessage = D._p.TriggerMessage
            };

          var dialog = Ui.Dialogs.SelectTarget.Create(selectTargetParameters);
          Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

          foreach (var target in dialog.Selection)
          {
            targets.AddEffect(target);
          }
        }

        Result = new ChosenTargets(targets);
      }

      private bool NoValidTargets(TargetValidator validator)
      {
        foreach (var target in GenerateTargets(validator.IsZoneValid))
        {
          if (validator.IsTargetValid(target, D._p.TriggerMessage))
            return false;
        }

        return true;
      }
    }
  }
}