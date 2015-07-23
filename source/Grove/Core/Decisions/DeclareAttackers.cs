namespace Grove.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Events;
  using UserInterface;
  using UserInterface.Messages;
  using UserInterface.SelectTarget;

  public class DeclareAttackers : Decision
  {
    private DeclareAttackers() {}

    public DeclareAttackers(Player controller)
      : base(
        controller, () => new UiHandler(), () => new MachineHandler(), () => new ScenarioHandler(),
        () => new PlaybackHandler()) {}

    private abstract class Handler : DecisionHandler<DeclareAttackers, ChosenAttackers>
    {
      private List<Card> _creaturesThatMustAttack;
      private List<Card> _planeswalkers;

      protected List<Card> CreaturesThatMustAttack
      {
        get
        {
          return _creaturesThatMustAttack ?? (_creaturesThatMustAttack = 
            D.Controller.Battlefield.CreaturesThatCanAttack
              .Where(x => x.Has().AttacksEachTurnIfAble && x.CombatCost == 0)
              .ToList());
        }
      }

      protected List<Card> Planeswalkers
      {
        get
        {
          return _planeswalkers ?? (_planeswalkers =
            D.Controller.Opponent.Battlefield.Where(x => x.Is().Planeswalker)
            .OrderBy(x => -x.Score)
            .ToList());
        }
      }      

      protected override bool ShouldExecuteQuery { get { return D.Controller.Battlefield.HasCreaturesThatCanAttack; } }

      public override void ProcessResults()
      {        
        foreach (var attacker in Result)
        {
          var combatCost = attacker.Card.CombatCost;
          if (attacker.Card.CanAttack && (combatCost == 0 || D.Controller.HasMana(combatCost)))
          {
            // pay for each attacker seperately
            // if there are some attackers which are
            // already tapped for mana, because we used them 
            // to pay combatcost for others, then they will
            // not be added.            
            if (combatCost > 0)
            {
              D.Controller.Consume(combatCost.Colorless(),
                ManaUsage.Any);
            }

            Combat.AddAttacker(attacker.Card, attacker.Planeswalker);
          }
        }

        Publish(new AttackersDeclaredEvent(Combat.Attackers));
      }

      protected override void SetResultNoQuery()
      {
        Result = new ChosenAttackers();
      }     
    }

    private class MachineHandler : Handler, ISearchNode, IMachineExecutionPlan
    {
      private readonly MachinePlanExecutor _executor;
      private List<ChosenAttackers> _declarations;

      public MachineHandler()
      {
        Result = new ChosenAttackers();
        _executor = new MachinePlanExecutor(this);
      }

      private Player Defender { get { return Controller.Opponent; } }

      public override bool HasCompleted { get { return _executor.HasCompleted; } }      

      void IMachineExecutionPlan.ExecuteQuery()
      {
        ExecuteQuery();
      }

      Game ISearchNode.Game { get { return Game; } }

      public Player Controller { get { return D.Controller; } }

      public int ResultCount { get { return _declarations.Count; } }

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
        base.Initialize();
      }

      protected override void ExecuteQuery()
      {
        Ai.SetBestResult(this);
      }

      private List<ChosenAttackers> GetAttackersDeclarations()
      {                
        var results = new List<ChosenAttackers>();
        
        // First choice: only attackers that must attack
        results.Add(SelectAttackTarget(CreaturesThatMustAttack));

        var everyCreatureThatCanAttack = RemoveAttackersIfWeCannotAffordToPayCombatCost(
          Controller.Battlefield.CreaturesThatCanAttack.ToList());

        if (everyCreatureThatCanAttack.Count > CreaturesThatMustAttack.Count)
        {
          var parameters = new AttackStrategyParameters
            {
              AttackerCandidates = everyCreatureThatCanAttack,
              BlockerCandidates = Defender.Battlefield.CreaturesThatCanBlock.ToList(),
              DefendingPlayersLife = Defender.Life
            };

          var greedyAttackers = new AttackStrategy(parameters)
            .ChooseAttackers()
            .Concat(CreaturesThatMustAttack)
            .Distinct()
            .ToList();          

          if (greedyAttackers.Count > CreaturesThatMustAttack.Count)
          {
            // Second choice: attackers selected by greedy attack strategy
            results.Add(SelectAttackTarget(greedyAttackers));
          }

          if (greedyAttackers.Count < everyCreatureThatCanAttack.Count)
          {            
            // Third choice: every creature that can attack
            results.Add(SelectAttackTarget(everyCreatureThatCanAttack));
          }
        }

        return results;
      }

      private ChosenAttackers SelectAttackTarget(List<Card> selectedAttackers)
      {
        // TODO improve 
        // Very simple strategy, every attacker attacks the most valuable planeswalker 
        // if any
        
        var planeswalker = Planeswalkers          
          .FirstOrDefault();

        return planeswalker == null
          ? new ChosenAttackers(selectedAttackers)
          : new ChosenAttackers(selectedAttackers, planeswalker);
      }

      private List<Card> RemoveAttackersIfWeCannotAffordToPayCombatCost(List<Card> allAttackers)
      {
        var attackersWithCombatCost = allAttackers
          .Where(x => x.CombatCost > 0)
          .OrderBy(x => -x.Power)
          .ToList();

        if (attackersWithCombatCost.Count == 0)
          return allAttackers;

        var availableMana = Controller.GetAvailableManaCount();

        foreach (var card in attackersWithCombatCost)
        {
          if (card.CombatCost <= availableMana)
          {
            availableMana -= card.CombatCost;
          }
          else
          {
            allAttackers.Remove(card);
          }
        }

        return allAttackers;
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        var chosenAttackers = Game.Recorder.LoadDecisionResult();

        if (chosenAttackers is ChosenAttackers)
        {
          Result = (ChosenAttackers) chosenAttackers;
        }
        else
        {
          // For backward compatibility reasons
          // so old savegames can be loaded.
          Result = new ChosenAttackers((ChosenCards) chosenAttackers);
        }
      }
    }

    private class ScenarioHandler : Handler
    {
      public ScenarioHandler()
      {
        Result = new ChosenAttackers();
      }

      protected override bool ShouldExecuteQuery { get { return true; } }

      protected override void ExecuteQuery()
      {
        Result = GetNextScenarioResult()
          ?? new ChosenAttackers();
      }
    }

    private class UiHandler : Handler
    {
      private bool IsValidAttackerDeclaration(ChosenAttackers attackers)
      {
        return CreaturesThatMustAttack.All(creature => attackers.Any(x => x.Card == creature));
      }

      protected override void ExecuteQuery()
      {
        var result = new ChosenAttackers();
        var availableMana = D.Controller.GetAvailableManaCount();

        while (true)
        {
          var attackerSpec = new IsValidTargetBuilder()
            .Is.Card(c => c.CanAttack && c.Controller == D.Controller && c.CombatCost <= availableMana)
            .On.Battlefield();

          var attackerTarget = new TargetValidatorParameters(
            isValidTarget: attackerSpec.IsValidTarget,
            isValidZone: attackerSpec.IsValidZone)
          {
            MinCount = IsValidAttackerDeclaration(result) ? 0 : 1,
            MaxCount = 1,
            Message = "Select an attacker.",
            MustBeTargetable = false
          };

          var validator = new TargetValidator(attackerTarget);
          validator.Initialize(Game, D.Controller);

          var selectAttacker = Ui.Dialogs.SelectTarget.Create(new SelectTargetParameters
          {
            Validator = validator,
            CanCancel = false,
            Instructions = IsValidAttackerDeclaration(result) ? null : "(Additional attackers required.)"
          });

          Ui.Shell.ShowModalDialog(selectAttacker, DialogType.Small, InteractionState.SelectTarget);

          if (selectAttacker.Selection.Count == 0)
          {
            break;
          }

          var attacker = (Card) selectAttacker.Selection[0];

          if (result.Any(a => a.Card == attacker))
          {
            availableMana += attacker.CombatCost;

            Ui.Publisher.Publish(
              new AttackerUnselected
                {
                  Attacker = attacker
                });


            result.Remove(attacker);
            continue;
          }

          Card planeswalker = null;
          if (Planeswalkers.Count > 0)
          {
            var planeswalkerSpec = new IsValidTargetBuilder()
              .Is.Card(c => c.Is().Planeswalker && c.Controller == D.Controller.Opponent)
              .On.Battlefield();

            var planeswalkerTarget = new TargetValidatorParameters(
              isValidTarget: planeswalkerSpec.IsValidTarget,
              isValidZone: planeswalkerSpec.IsValidZone)
              {
                MinCount = 0,
                MaxCount = 1,
                Message = "Select a planeswalker to attack",
                MustBeTargetable = false
              };

            var planeswalkerValidator = new TargetValidator(planeswalkerTarget);
            planeswalkerValidator.Initialize(Game, D.Controller);

            var selectPlaneswalker = Ui.Dialogs.SelectTarget.Create(new SelectTargetParameters
            {
              Validator = planeswalkerValidator,              
              Instructions = "(or press enter to attack the opponent)."
            });

            Ui.Shell.ShowModalDialog(selectPlaneswalker, DialogType.Small, InteractionState.SelectTarget);
            planeswalker = (Card)selectPlaneswalker.Selection[0];
          }

          availableMana -= attacker.CombatCost;          
          Ui.Publisher.Publish(
            new AttackerSelected
              {
                Attacker = attacker
              });

          result.Add(attacker, planeswalker);
        }                                      

        Result = result;
      }
    }
  }
}