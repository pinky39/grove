namespace Grove.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Infrastructure;
  using UserInterface;
  using UserInterface.Messages;
  using UserInterface.SelectTarget;

  public class DeclareBlockers : Decision
  {
    private DeclareBlockers() {}

    public DeclareBlockers(Player controller)
      : base(controller, () => new UiHandler(), () => new MachineHandler(),
        () => new ScenarioHandler(), () => new PlaybackHandler()) {}

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

    private class MachineHandler : Handler, IMachineExecutionPlan, ISearchNode
    {
      private readonly MachinePlanExecutor _executor;
      private List<ChosenBlockers> _declarations;

      public MachineHandler()
      {
        Result = new ChosenBlockers();
        _executor = new MachinePlanExecutor(this);
      }

      public override bool HasCompleted
      {
        get { return _executor.HasCompleted; }
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
        get { return _declarations.Count; }
      }

      public void SetResult(int index)
      {
        Result = _declarations[index];
      }

      public void GenerateChoices()
      {
        _declarations = GetBlockersDeclarations();
      }

      protected override void Initialize()
      {
        _executor.Initialize(ChangeTracker);
      }

      private void GetBlockersDeclarationsNormal(List<Card> attackers, List<ChosenBlockers> strategies)
      {
        // 1. Strategy, no blockers
        strategies.Add(ChosenBlockers.None);

        // 2. Strategy, try assign some blockers via shallow strategy        
        var strategy2 = BlockStrategy.ChooseBlockers(new BlockStrategyParameters
          {
            Attackers = attackers,
            BlockerCandidates = D.Controller.Battlefield.CreaturesThatCanBlock.ToList(),
            DefendersLife = D.Controller.Life
          });

        if (strategy2.Count > 0)
        {
          strategies.Add(strategy2);
        }
      }

      private void GetBlockersDeclarationsLure(List<Card> attackers, List<ChosenBlockers> strategies)
      {
        var chosenBlockers = new ChosenBlockers();
        var candidates = new HashSet<Card>(D.Controller.Battlefield.CreaturesThatCanBlock);

        foreach (var attacker in attackers.Where(x => x.Has().Lure).OrderBy(x => x.Toughness))
        {
          foreach (var blocker in candidates.Where(x => attacker.CanBeBlockedBy(x)).ToList())
          {
            chosenBlockers.Add(blocker, attacker);
            candidates.Remove(blocker);
          }
        }

        if (candidates.Count > 0)
        {
          var remaining = BlockStrategy.ChooseBlockers(new BlockStrategyParameters
            {
              Attackers = attackers,
              BlockerCandidates = candidates.ToList(),
              DefendersLife = D.Controller.Life
            });

          foreach (var attackerBlockerPair in remaining)
          {
            chosenBlockers.Add(attackerBlockerPair.Blocker, attackerBlockerPair.Attacker);
          }
        }

        strategies.Add(chosenBlockers);
      }

      private List<ChosenBlockers> GetBlockersDeclarations()
      {
        var strategies = new List<ChosenBlockers>();
        var attackers = Combat.Attackers.Select(x => x.Card).ToList();

        if (attackers.Any(x => x.Has().Lure))
        {
          GetBlockersDeclarationsLure(attackers, strategies);
        }
        else
        {
          GetBlockersDeclarationsNormal(attackers, strategies);
        }

        return strategies;
      }

      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }

      public override void Execute()
      {
        _executor.Execute();
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery
      {
        get { return true; }
      }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenBlockers) Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      protected override bool ShouldExecuteQuery
      {
        get { return true; }
      }

      protected override void ExecuteQuery()
      {
        Result = GetNextScenarioResult()
          ?? ChosenBlockers.None;
      }
    }

    private class UiHandler : Handler
    {
      private bool IsLureAbilitySatisfied(ChosenBlockers chosen, List<Card> lureAttackers)
      {
        return lureAttackers.Count == 0 ||
          D.Controller.Battlefield.All(c =>
            lureAttackers.None(a => a.CanBeBlockedBy(c)) ||
              chosen.ContainsBlocker(c));
      }

      private bool IsMinimumBlockerCountSatisfied(ChosenBlockers chosen)
      {
        var attackers = chosen
          .GroupBy(x => x.Attacker)
          .Select(x => new
            {
              Card = x.Key,
              BlockerCount = x.Count()
            })
          .ToList();

        return attackers.All(attacker => attacker.Card.MinimalBlockerCount <= attacker.BlockerCount);
      }

      private bool IsValidBlockerDeclaration(ChosenBlockers chosen,
        List<Card> lureAttackers)
      {
        return IsMinimumBlockerCountSatisfied(chosen) &&
          IsLureAbilitySatisfied(chosen, lureAttackers);
      }

      protected override void ExecuteQuery()
      {
        var result = new ChosenBlockers();
        
        var lureAttackers = Combat.Attackers
          .Select(x => x.Card)
          .Where(x => x.Has().Lure)
          .ToList();

        while (true)
        {
          var blockerTarget = new TargetValidatorParameters
            {
              MinCount = IsValidBlockerDeclaration(result, lureAttackers) ? 0 : 1,
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
              Instructions = IsValidBlockerDeclaration(result, lureAttackers) ? null : "(Additional blockers required.)"
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

          var attackerTarget = new TargetValidatorParameters
            {
              MinCount = 1,
              MaxCount = 1,
              Message = "Select an attacker to block."
            }
            .Is.Card(c =>
              {
                // if any attacker has lure, we must block it
                if (lureAttackers.Any(x => x.CanBeBlockedBy(blocker)) && !c.Has().Lure)
                {
                  return false;
                }

                return c.IsAttacker && c.CanBeBlockedBy(blocker);
              })
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