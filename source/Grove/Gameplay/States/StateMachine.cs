namespace Grove.Gameplay.States
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;
  using Messages;
  using Misc;

  public class StateMachine : GameObject, ICopyContributor
  {
    private readonly Trackable<IDecision> _curentDecision = new Trackable<IDecision>();
    private readonly DecisionQueue _decisionQueue;
    private Trackable<StateNode> _currentState = new Trackable<StateNode>();
    private Trackable<StepNode> _currentStep = new Trackable<StepNode>();
    private Player _previousLooser;
    private List<StepNode> _steps;

    private StateMachine() {}

    public StateMachine(DecisionQueue decisionQueue)
    {
      _decisionQueue = decisionQueue;
    }

    private IDecision CurrentDecision { get { return _curentDecision.Value; } set { _curentDecision.Value = value; } }
    public bool WasPriorityPassed { get { return CurrentDecision.IsPass; } }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      _steps = CreateStepNodes();

      var currentStep = _steps.First(x => x.Step == Turn.Step);
      var currentState = currentStep.States.First(x => x.Id == Turn.State);

      _currentStep = new Trackable<StepNode>(currentStep);
      _currentState = new Trackable<StateNode>(currentState);

      _currentStep.Initialize(ChangeTracker);
      _currentState.Initialize(ChangeTracker);
    }

    public void Initialize(Game game)
    {
      Game = game;

      _curentDecision.Initialize(ChangeTracker);
      _currentStep.Initialize(ChangeTracker);
      _currentState.Initialize(ChangeTracker);
      _steps = CreateStepNodes();
    }

    public void Resume(Func<bool> shouldContinue)
    {
      while (ExecutePendingDecisions(shouldContinue))
      {
        var nextState = _currentState.Value.Next();

        if (nextState == null)
        {
          var nextStep = _currentStep.Value.Next();

          while (Players.Active.ShouldSkipStep(nextStep.Step))
          {
            nextStep = nextStep.Next();
          }

          _currentStep.Value = nextStep;
          nextState = nextStep.Start;
          Turn.Step = nextStep.Step;
        }

        _currentState.Value = nextState;
        Turn.State = nextState.Id;

        nextState.Execute();
      }      
    }

    public void Start(Func<bool> shouldContinue, bool skipPreGame, Player looser = null)
    {
      Turn.Step = Step.GameStart;

      if (skipPreGame)
      {
        Turn.Step = Step.Untap;
      }

      Turn.State = 0;
      _previousLooser = looser;

      _currentStep.Value = _steps.First(x => x.Step == Turn.Step);
      _currentState.Value = _currentStep.Value.Start;

      _currentState.Value.Execute();

      Resume(shouldContinue);
    }

    private void DeclareAttackers()
    {
      Enqueue<DeclareAttackers>(
        controller: Players.Attacking);
    }

    private void DeclareBlockers()
    {
      Enqueue<DeclareBlockers>(
        controller: Players.Defending);
    }

    private void DiscardToMaximumHandSize()
    {
      Enqueue<DiscardCards>(
        controller: Players.Active,
        init: p => p.Count = Players.Active.NumberOfCardsAboveMaximumHandSize);
    }

    private void DrawStartingHands()
    {
      foreach (var player in Players)
      {
        player.DrawStartingHand();
      }
    }

    private bool ExecutePendingDecisions(Func<bool> shouldContinue)
    {
      var resultsToSave = new List<IDecision>();

      var should = false;

      while (shouldContinue())
      {
        if (CurrentDecision == null || CurrentDecision.HasCompleted)
        {
          if (_decisionQueue.Count == 0)
          {
            should = true;
            break;
          }

          CurrentDecision = _decisionQueue.Dequeue();
        }

        CurrentDecision.Execute();
        resultsToSave.Add(CurrentDecision);
      }

      // it's important to save the results only when decision queue is empty
      // if its not the playback will not work, because the decisions not yet
      // executed, can not be saved properly
      foreach (var decision in resultsToSave)
      {
        decision.SaveDecisionResults();
      }

      return should;
    }

    private List<StepNode> CreateStepNodes()
    {
      StepNode gameStart = null,
               mulligan = null,
               untap = null,
               upkeep = null,
               draw = null,
               firstMain = null,
               beginOfCombat = null,
               declareAttackers = null,
               declareBlockers = null,
               firstStrikeCombatDamage = null,
               combatDamage = null,
               endofCombat = null,
               secondMain = null,
               endOfTurn = null,
               cleanup = null;

      gameStart = NewStep(Step.GameStart,
        new Action[]
          {
            SelectStartingPlayer,
            ShuffleLibraries,
            DrawStartingHands,
          },
        next: () => mulligan,
        getsPriority: false);

      mulligan = NewStep(Step.Mulligan,
        new Action[] {TakeMulligans},
        next: () => Players.AnotherMulliganRound ? mulligan : untap,
        getsPriority: false);

      untap = NewStep(Step.Untap,
        new Action[]
          {
            () => Turn.NextTurn(),
            () =>
              {
                foreach (var permanent in Players.Active.Battlefield)
                {
                  permanent.HasSummoningSickness = false;

                  if (permanent.Has().DoesNotUntap)
                    continue;

                  if (permanent.MayChooseNotToUntap)
                  {
                    var permanentCopy = permanent;
                    Enqueue<ChooseToUntap>(
                      controller: Players.Active,
                      init: p => p.Permanent = permanentCopy);
                  }
                  else
                  {
                    permanent.Untap();
                  }
                }

                Players.Active.LandsPlayedCount = 0;
              }
          },
        next: () => upkeep,
        getsPriority: false);

      upkeep = NewStep(Step.Upkeep,
        next: () => draw,
        getsPriority: true);


      draw = NewStep(Step.Draw,
        new Action[]
          {
            () =>
              {
                if (Turn.TurnCount != 1)
                {
                  Players.Active.DrawCard();
                }
              }
          },
        next: () => firstMain,
        getsPriority: true);

      firstMain = NewStep(Step.FirstMain,
        next: () => beginOfCombat,
        getsPriority: true);

      beginOfCombat = NewStep(Step.BeginningOfCombat,
        next: () => declareAttackers,
        getsPriority: true);

      declareAttackers = NewStep(Step.DeclareAttackers,
        new Action[]
          {
            DeclareAttackers
          },
        next: () => declareBlockers,
        getsPriority: true);

      declareBlockers = NewStep(Step.DeclareBlockers,
        new Action[]
          {
            DeclareBlockers,
            SetDamageAssignmentOrder,
          },
        next: () => Combat.AnyCreaturesWithFirstStrike()
          ? firstStrikeCombatDamage
          : combatDamage,
        getsPriority: true);


      firstStrikeCombatDamage = NewStep(Step.FirstStrikeCombatDamage,
        new Action[]
          {
            () => Combat.AssignCombatDamage(firstStrike: true),
            DealAssignedCombatDamage,
            () => Players.MoveDeadCreaturesToGraveyard()
          },
        next: () => Combat.AnyCreaturesWithNormalStrike()
          ? combatDamage
          : endofCombat,
        getsPriority: true);

      combatDamage = NewStep(Step.CombatDamage,
        new Action[]
          {
            () => Combat.AssignCombatDamage(),
            DealAssignedCombatDamage,
            () => Players.MoveDeadCreaturesToGraveyard()
          },
        next: () => endofCombat,
        getsPriority: true);

      endofCombat = NewStep(Step.EndOfCombat,
        new Action[]
          {
            () => Combat.RemoveAll()
          },
        next: () => secondMain,
        getsPriority: true);

      secondMain = NewStep(Step.SecondMain,
        next: () => endOfTurn,
        getsPriority: true);

      endOfTurn = NewStep(Step.EndOfTurn,
        next: () => cleanup,
        getsPriority: true);

      cleanup = NewStep(Step.CleanUp,
        new Action[]
          {
            () =>
              {
                Players.RemoveDamageFromPermanents();
                Players.RemoveRegenerationFromPermanents();

                DiscardToMaximumHandSize();
                Publish(new EndOfTurn());
              },
            () => Players.ChangeActivePlayer(),
          },
        next: () => untap,
        getsPriority: false);


      var all = new List<StepNode>
        {
          gameStart,
          mulligan,
          untap,
          upkeep,
          draw,
          firstMain,
          beginOfCombat,
          declareAttackers,
          declareBlockers,
          firstStrikeCombatDamage,
          combatDamage,
          endofCombat,
          secondMain,
          endOfTurn,
          cleanup
        };

      return all;
    }

    private StepNode NewStep(Step step, IEnumerable<Action> before, bool getsPriority, Func<StepNode> next)
    {
      return new StepNode(step, CreateStates(before, getsPriority), next);
    }

    private StepNode NewStep(Step step, bool getsPriority, Func<StepNode> next)
    {
      return new StepNode(step, CreateStates(new Action[] {}, getsPriority), next);
    }

    private void DealAssignedCombatDamage()
    {
      Combat.DealAssignedDamage();

      if (Combat.AnyCreaturesWithFirstStrike() && Turn.Step == Step.FirstStrikeCombatDamage)
        Publish(new AssignedCombatDamageWasDealt(Turn.Step));

      if (Combat.AnyCreaturesWithNormalStrike() && Turn.Step == Step.CombatDamage)
        Publish(new AssignedCombatDamageWasDealt(Turn.Step));
    }

    private Player RollDice()
    {
      int result1;
      int result2;

      while (true)
      {
        result1 = RollADice();
        result2 = RollADice();

        if (result1 != result2)
          break;
      }

      return result1 > result2
        ? Players.Player1
        : Players.Player2;
    }

    private void SelectStartingPlayer()
    {
      var winner = _previousLooser ?? RollDice();
      Enqueue<SelectStartingPlayer>(winner);
    }

    private void SetDamageAssignmentOrder()
    {
      Combat.SetDamageAssignmentOrder();
    }

    private void ShuffleLibraries()
    {
      foreach (var player in Players)
      {
        player.ShuffleLibrary();
      }
    }

    private void TakeMulligans()
    {
      var starting = Players.Starting;
      var nonStarting = starting.Opponent;

      Enqueue<TakeMulligan>(starting);
      Enqueue<TakeMulligan>(nonStarting);
    }

    private List<StateNode> CreateStates(IEnumerable<Action> before, bool getsPriority)
    {
      var nodes = new List<StateNode>();

      StateNode startOfStep = null,
                priorityActiveStart = null,
                priorityPassiveStart = null,
                priorityActive = null,
                priorityPassive = null,
                priorityAfter = null,
                priorityBeginResolve = null,
                priorityFinishResolve = null;

      if (getsPriority)
      {
        priorityActiveStart = new StateNode(
          100,
          () => Enqueue<PlaySpellOrAbility>(Players.Active),
          () =>
            {
              if (WasPriorityPassed)
              {
                return priorityPassive;
              }

              return priorityActiveStart;
            }
          );

        nodes.Add(priorityActiveStart);

        priorityActive = new StateNode(
          101,
          () => Enqueue<PlaySpellOrAbility>(Players.Active),
          () =>
            {
              if (WasPriorityPassed)
              {
                return priorityBeginResolve;
              }

              return priorityActiveStart;
            });

        nodes.Add(priorityActive);

        priorityPassive = new StateNode(
          102,
          () => Enqueue<PlaySpellOrAbility>(Players.Passive),
          () =>
            {
              if (WasPriorityPassed)
              {
                return Stack.IsEmpty
                  ? priorityAfter
                  : priorityBeginResolve;
              }

              return priorityPassiveStart;
            }
          );

        nodes.Add(priorityPassive);

        priorityPassiveStart = new StateNode(
          103,
          () => Enqueue<PlaySpellOrAbility>(Players.Passive),
          () =>
            {
              if (WasPriorityPassed)
              {
                return priorityActive;
              }

              return priorityPassiveStart;
            }
          );

        nodes.Add(priorityPassiveStart);

        priorityBeginResolve = new StateNode(
          104,
          () => Stack.Resolve(),
          () => priorityFinishResolve);

        nodes.Add(priorityBeginResolve);

        priorityFinishResolve = new StateNode(
          105,
          () =>
            {
              var effect = Stack.LastResolved;
              if (effect != null)
              {
                effect.FinishResolve();
              }
            },
          () => priorityActiveStart);

        nodes.Add(priorityFinishResolve);
      }

      priorityAfter = new StateNode(
        106,
        () =>
          {
            Players.Player1.EmptyManaPool();
            Players.Player2.EmptyManaPool();

            Publish(new StepFinished
              {
                Step = Turn.Step
              });
          },
        () => null);
      
      nodes.Add(priorityAfter);
      var nextNode = getsPriority ? priorityActiveStart : priorityAfter;

      // create and connect step custom statenodes
      var id = 1;
      foreach (var action in before.Reverse())
      {
        var currentNextNode = nextNode;
        nextNode = new StateNode(id, action, () => currentNextNode);

        nodes.Add(nextNode);
        id++;
      }

      startOfStep = new StateNode(
        0,
        () => Publish(new StepStarted(Turn.Step)),
        () => nextNode);

      nodes.Add(startOfStep);

      return nodes;
    }
  }

  public class StateNode
  {
    public readonly Action Execute;
    public readonly int Id;
    public readonly Func<StateNode> Next;

    public StateNode(int id, Action action, Func<StateNode> next)
    {
      Id = id;
      Execute = action;
      Next = next;
    }
  }

  public class StepNode
  {
    public readonly Func<StepNode> Next;
    public readonly List<StateNode> States;
    public readonly Step Step;

    public StepNode(Step step, List<StateNode> states, Func<StepNode> next)
    {
      Step = step;
      Next = next;
      States = states;
    }

    public StateNode Start { get { return States[States.Count - 1]; } }
  }
}