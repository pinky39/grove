namespace Grove.Gameplay.Decisions
{
  using System.Linq;
  using AI;
  using UserInterface;
  using UserInterface.Messages;
  using UserInterface.SelectTarget;

  public class DeclareBlockers : Decision
  {
    private DeclareBlockers() {}

    public DeclareBlockers(Player controller)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(), () => new PlaybackHandler()) {}

    private abstract class Handler : DecisionHandler<DeclareBlockers, ChosenBlockers>
    {
      protected override bool ShouldExecuteQuery
      {
        get
        {
          return Game.Combat.CanAnyAttackerBeBlockedByAny(
            D.Controller.Battlefield.Creatures);
        }
      }

      public override void ProcessResults()
      {
        foreach (var pair in Result)
        {
          Combat.DeclareBlocker(pair.Blocker, pair.Attacker);
        }
      }

      protected override void SetResultNoQuery()
      {
        Result = new ChosenBlockers();
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new ChosenBlockers();
      }

      protected override void ExecuteQuery()
      {
        var p = new BlockStrategyParameters
          {
            Attackers = Combat.Attackers.Select(x => x.Card).ToList(),
            BlockerCandidates = D.Controller.Battlefield.CreaturesThatCanBlock.ToList(),
            DefendersLife = D.Controller.Life
          };

        Result = new BlockStrategy(p).ChooseBlockers();
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenBlockers) Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      protected override void ExecuteQuery()
      {
        Result = ExecuteAssertionsAndGetNextScenarioResult()
          ?? ChosenBlockers.None;
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = new ChosenBlockers();

        while (true)
        {
          var blockerTarget = new TargetValidatorParameters
            {
              MinCount = result.IsValid() ? 0 : 1,
              MaxCount = 1,
              Message = "Select a blocker."
            }
            .Is.Card(c => c.CanBlock() && c.Controller == D.Controller)
            .On.Battlefield();

          blockerTarget.MustBeTargetable = false;

          var blockerValidator = new TargetValidator(blockerTarget);
          blockerValidator.Initialize(Game, D.Controller);

          var selectBlocker = Ui.Dialogs.SelectTarget.Create(new SelectTargetParameters
            {
              Validator = blockerValidator,
              CanCancel = false,
              Instructions = result.IsValid() ? null : "(Additional blockers required.)"
            });

          Ui.Shell.ShowModalDialog(selectBlocker, DialogType.Small, InteractionState.SelectTarget);

          if (selectBlocker.Selection.Count == 0)
          {
            break;
          }

          var blocker = (Card) selectBlocker.Selection[0];

          if (result.ContainsBlocker(blocker))
          {
            result.Remove(blocker);

            Ui.Publisher.Publish(new BlockerUnselected
              {
                Blocker = blocker
              });

            continue;
          }

          var attackerTarget =
            new TargetValidatorParameters {MinCount = 1, MaxCount = 1, Message = "Select an attacker to block."}
              .Is.Card(c => c.IsAttacker && c.CanBeBlockedBy(blocker))
              .On.Battlefield();

          attackerTarget.MustBeTargetable = false;

          var attackerValidator = new TargetValidator(attackerTarget);
          attackerValidator.Initialize(Game, D.Controller);

          var selectAttacker = Ui.Dialogs.SelectTarget.Create(new SelectTargetParameters
            {
              Validator = attackerValidator,
              CanCancel = true,
              Instructions = "(Press Esc to cancel.)"
            });

          Ui.Shell.ShowModalDialog(selectAttacker, DialogType.Small, InteractionState.SelectTarget);

          if (selectAttacker.WasCanceled)
            continue;

          var attacker = (Card) selectAttacker.Selection[0];

          Ui.Publisher.Publish(new BlockerSelected
            {
              Blocker = blocker,
              Attacker = attacker
            });

          result.Add(blocker, attacker);
        }

        Result = result;
      }
    }
  }
}