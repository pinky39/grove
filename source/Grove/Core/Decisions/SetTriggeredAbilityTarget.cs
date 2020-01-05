namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Infrastructure;
  using UserInterface;
  using UserInterface.SelectTarget;

  public class SetTriggeredAbilityTarget : Decision
  {
    private readonly Params _p = new Params();

    private SetTriggeredAbilityTarget() {}

    public SetTriggeredAbilityTarget(Player controller, Action<Params> setParameters) : base(controller,
      () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<SetTriggeredAbilityTarget, ChosenTargets>
    {
      public override void ProcessResults()
      {        
        var effect = D._p.Effect;        

        if (Result.HasTargets == false)
        {          
          effect.EffectCountered(SpellCounterReason.IllegalTarget);
          return;
        }

        effect.SetTriggeredAbilityTargets(Result.Targets);

        if (D._p.UsesStack)
        {
          Stack.QueueTriggered(effect);
        }
        else
        {
          effect.BeginResolve();
          effect.FinishResolve();
        }        
      }

      protected override void SetResultNoQuery()
      {
        Result = new ChosenTargets(null);
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

      public override bool HasCompleted { get { return _executor.HasCompleted; } }           

      void IMachineExecutionPlan.ExecuteQuery()
      {
        ExecuteQuery();
      }

      Game ISearchNode.Game { get { return Game; } }

      public Player Controller { get { return D.Controller; } }

      public int ResultCount { get { return _targets.Count; } }

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
          D._p.Effect.Source.OwningCard,
          D._p.TargetSelector,
          D._p.MachineRules.Where(x => x is TargetingRule).Cast<TargetingRule>(),
          D._p.DistributeAmount,
          force: true,
          triggerMessage: D._p.Effect.TriggerMessage<object>());

        return targetsCandidates.Take(Ai.CurrentTargetCount);
      }
    }

    [Copyable]
    public class Params
    {
      public Effect Effect;      
      public List<MachinePlayRule> MachineRules;
      public TargetSelector TargetSelector;
      public bool UsesStack;
      public int DistributeAmount;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenTargets) Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        Result = GetNextScenarioResult() ?? ChosenTargets.None();
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
              TriggerMessage = D._p.Effect.TriggerMessage<object>()
            };

          var dialog = Ui.Dialogs.SelectTarget.Create(selectTargetParameters);
          Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

          foreach (var target in dialog.Selection)
          {
            targets.AddEffect(target);
          }
        }

        if (D._p.DistributeAmount > 0)
        {
          targets.Distribution = DistributeAmount(targets.Effect, D._p.DistributeAmount);
        }

        Result = new ChosenTargets(targets);
      }

      private List<int> DistributeAmount(IList<ITarget> targets, int amount)
      {
        if (targets.Count == 1)
        {
          return new List<int> { amount };
        }

        var dialog = Ui.Dialogs.DistributeAmount.Create(targets, amount);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

        return dialog.Distribution;
      }

      private bool NoValidTargets(TargetValidator validator)
      {
        foreach (var target in GenerateTargets(validator.IsZoneValid))
        {
          if (validator.IsTargetValid(target, D._p.Effect.TriggerMessage<object>()))
            return false;
        }

        return true;
      }
    }
  }
}