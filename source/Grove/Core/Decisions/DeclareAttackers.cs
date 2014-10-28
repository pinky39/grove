namespace Grove.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.AI;
  using Grove.UserInterface;
  using Grove.UserInterface.Messages;
  using Grove.UserInterface.SelectTarget;

  public class DeclareAttackers : Decision
  {
    private DeclareAttackers()
    {
    }

    public DeclareAttackers(Player controller)
      : base(
        controller, () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(),
        () => new PlaybackHandler())
    {
    }

    private abstract class Handler : DecisionHandler<DeclareAttackers, ChosenCards>
    {
      protected override bool ShouldExecuteQuery
      {
        get { return D.Controller.Battlefield.HasCreaturesThatCanAttack; }
      }

      public override void ProcessResults()
      {
        AddCreaturesThatMustAttack();

        foreach (var attacker in Result)
        {
          if (attacker.CanAttack)
          {
            Combat.AddAttacker(attacker);
          }
        }
      }

      protected override void SetResultNoQuery()
      {
        Result = new ChosenCards();
      }

      private void AddCreaturesThatMustAttack()
      {
        var creaturesThatMustAttack = D.Controller.Battlefield
          .CreaturesThatCanAttack.Where(x => x.Has().AttacksEachTurnIfAble);

        foreach (var creature in creaturesThatMustAttack)
        {
          if (!Result.Contains(creature))
          {
            Result.Add(creature);
          }
        }
      }
    }

    private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
    {
      private readonly MachinePlanExecutor _executor;
      private List<List<Card>> _declarations;

      public MachineHandler()
      {
        Result = new ChosenCards();
        _executor = new MachinePlanExecutor(this);
      }

      private Player Defender
      {
        get { return Controller.Opponent; }
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

      public void GenerateChoices()
      {
        _declarations = GetAttackersDeclarations();
      }

      public void SetResult(int index)
      {
        Result = _declarations[index];
      }


      public override void Execute()
      {
        _executor.Execute();
      }

      public override string ToString()
      {
        return string.Format("{0}: {1} declares attackers", Game.Turn.Step, Controller);
      }

      protected override void Initialize()
      {
        _executor.Initialize(ChangeTracker);
      }

      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }

      private List<List<Card>> GetAttackersDeclarations()
      {
        var results = new List<List<Card>>();

        var noAttackers = new List<Card>();

        results.Add(noAttackers);

        var allAttackers = Controller.Battlefield.CreaturesThatCanAttack.ToList();

        if (allAttackers.Count > 0)
        {
          var parameters = new AttackStrategyParameters
            {
              AttackerCandidates = allAttackers,
              BlockerCandidates = Defender.Battlefield.CreaturesThatCanBlock.ToList(),
              DefendingPlayersLife = Defender.Life
            };

          var chosenAttackers = new AttackStrategy(parameters).ChooseAttackers();

          if (chosenAttackers.Count > 0)
          {
            results.Add(chosenAttackers);
          }

          if (chosenAttackers.Count < allAttackers.Count)
          {
            results.Add(allAttackers);
          }
        }

        return results;
      }
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
        Result = (ChosenCards) Game.Recorder.LoadDecisionResult();
      }
    }

    private class ScenarioHandler : Handler
    {
      public ScenarioHandler()
      {
        Result = new ChosenCards();
      }

      protected override bool ShouldExecuteQuery
      {
        get { return true; }
      }

      protected override void ExecuteQuery()
      {
        Result = GetNextScenarioResult()
          ?? ChosenCards.None;
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var tp = new TargetValidatorParameters {MinCount = 0, MaxCount = null, Message = "Select attackers."}
          .Is.Card(c => c.CanAttack && c.Controller == D.Controller)
          .On.Battlefield();

        tp.MustBeTargetable = false;

        var validator = new TargetValidator(tp);
        validator.Initialize(Game, D.Controller);

        var selectParameters = new SelectTargetParameters
          {
            Validator = validator,
            CanCancel = false,
            TargetSelected = target => Ui.Publisher.Publish(
              new AttackerSelected
                {
                  Attacker = target.Card()
                }),
            TargetUnselected = target => Ui.Publisher.Publish(
              new AttackerUnselected
                {
                  Attacker = target.Card()
                })
          };

        var dialog = Ui.Dialogs.SelectTarget.Create(selectParameters);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
        Result = dialog.Selection.ToList();
      }
    }
  }
}