namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Controllers;
  using Infrastructure;
  using log4net;
  using Messages;
  using Zones;

  [Copyable]
  public class StateMachine : ICopyContributor
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (StateMachine));
    private readonly Combat _combat;
    private readonly Trackable<IDecision> _curentDecision;
    private readonly Decisions _decisions;
    private readonly DecisionQueue _decisionsQueue;
    private readonly Players _players;
    private readonly Publisher _publisher;
    private readonly Stack _stack;
    private readonly TurnInfo _turnInfo;
    private Player _looser;

    private Dictionary<State, StepState> _states;
    private Dictionary<Step, StepDefinition> _steps;

    private StateMachine() {}

    public StateMachine(DecisionQueue decisionsQueue, Players players, Stack stack,
                        ChangeTracker changeTracker, Publisher publisher, Combat combat, Decisions decisions,
                        TurnInfo turnInfo)
    {
      _decisionsQueue = decisionsQueue;
      _stack = stack;
      _publisher = publisher;
      _combat = combat;
      _decisions = decisions;
      _turnInfo = turnInfo;
      _players = players;
      _curentDecision = new Trackable<IDecision>(changeTracker);

      InitializeStepStates();
      InitializeSteps();
    }

    private IDecision CurrentDecision
    {
      get { return _curentDecision.Value; }
      set { _curentDecision.Value = value; }
    }

    private State State
    {
      get { return _turnInfo.State; }
      set
      {
        _turnInfo.State = value;
        Log.DebugFormat("State: {0}", value);
      }
    }

    private Step Step
    {
      get { return _turnInfo.Step; }
      set { _turnInfo.Step = value; }
    }

    private bool WasPriorityPassed
    {
      get { return CurrentDecision.WasPriorityPassed; }
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      InitializeStepStates();
      InitializeSteps();
    }

    public void Resume(Func<bool> shouldContinue)
    {
      while (shouldContinue())
      {
        ExecutePendingDecisions();

        if (shouldContinue() == false)
          break;

        State = _states[State].Next;
        _states[State].Execute();
      }
    }

    public void Start(Func<bool> shouldContinue, bool skipPreGame, Player looser = null)
    {
      Step = skipPreGame ? Step.Untap : Step.GameStart;
      State = State.Begin;
      _looser = looser;

      _states[State].Execute();
      Resume(shouldContinue);
    }

    private void CreateState(State name, Action proc, Func<State> next)
    {
      _states[name] = new StepState(name, proc, next);
    }

    private void CreateStep(
      Step step,
      Func<Step> nextStep,
      bool getPriority = true,
      Action first = null,
      Action second = null,
      Action third = null)
    {
      _steps[step] = new StepDefinition(step, nextStep, getPriority, first, second, third);
    }

    private void DeclareAttackers()
    {
      _decisions.EnqueueDeclareAttackers(_players.Attacking);
    }

    private void DeclareBlockers()
    {
      _decisions.EnqueueDeclareBlockers(_players.Defending);
    }

    private void DiscardToMaximumHandSize()
    {
      _decisions.EnqueueDiscardCards(_players.Active,
        _players.Active.NumberOfCardsAboveMaximumHandSize);
    }

    private void DrawStartingHands()
    {
      foreach (var player in _players)
      {
        player.DrawStartingHand();
      }
    }

    private void ExecutePendingDecisions()
    {
      while (true)
      {
        if (CurrentDecision == null || CurrentDecision.HasCompleted)
        {
          if (_decisionsQueue.Count == 0)
            break;

          CurrentDecision = _decisionsQueue.Dequeue();
        }

        CurrentDecision.Execute();
      }
    }

    private void InitializeStepStates()
    {
      _states = new Dictionary<State, StepState>();

      CreateState(
        name: State.Begin,
        proc: () => Publish(new StepStarted(Step)),
        next: () => State.First);

      CreateState(
        name: State.First,
        proc: () => _steps[Step].First(),
        next: () => State.Second);

      CreateState(
        name: State.Second,
        proc: () => _steps[Step].Second(),
        next: () => State.Third);

      CreateState(
        name: State.Third,
        proc: () => _steps[Step].Third(),
        next: () => _steps[Step].GetPriority ? State.Start : State.After
        );

      CreateState(
        name: State.Start,
        proc: () => _decisions.EnqueuePlaySpellOrAbility(_players.Active),
        next: () =>
          WasPriorityPassed ? State.Passive : State.Active
        );

      CreateState(
        name: State.Active,
        proc: () => _decisions.EnqueuePlaySpellOrAbility(_players.Active),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              if (_stack.IsEmpty)
                return State.After;

              return _stack.TopSpellOwner.IsActive
                ? State.Passive
                : State.BeginResolve;
            }

            return State.Active;
          });

      CreateState(
        name: State.Passive,
        proc: () => _decisions.EnqueuePlaySpellOrAbility(_players.Passive),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              if (_stack.IsEmpty)
                return State.After;

              return _stack.TopSpellOwner.IsActive
                ? State.BeginResolve
                : State.Active;
            }

            return State.Passive;
          });

      CreateState(
        name: State.BeginResolve,
        proc: () => _stack.Resolve(),
        next: () => State.FinishResolve);

      CreateState(
        name: State.FinishResolve,
        proc: () =>
          {
            var effect = _stack.LastResolved;
            if (effect != null)
            {
              _players.MoveDeadCreaturesToGraveyard();
              effect.EffectWasResolved();
            }
          },
        next: () => State.Start);

      CreateState(
        name: State.After,
        proc: () =>
          {
            _players.Player1.EmptyManaPool();
            _players.Player2.EmptyManaPool();

            Publish(new TurnStepFinished
              {
                Step = Step
              });
          },
        next: () => State.End
        );

      CreateState(
        name: State.End,
        proc: () => { Step = _steps[Step].Next; },
        next: () => State.Begin);
    }


    private void InitializeSteps()
    {
      _steps = new Dictionary<Step, StepDefinition>();

      CreateStep(
        Step.GameStart,
        getPriority: false,
        first: SelectStartingPlayer,
        second: ShuffleLibraries,
        third: DrawStartingHands,
        nextStep: () => Step.Mulligan);

      CreateStep(
        Step.Mulligan,
        getPriority: false,
        first: TakeMulligans,
        nextStep: () => _players.AnotherMulliganRound ? Step.Mulligan : Step.Untap);

      CreateStep(
        Step.Untap,
        getPriority: false,
        first: () => Publish(new TurnStarted {TurnCount = _turnInfo.TurnCount}),
        second: () =>
          {
            foreach (var permanent in _players.Active.Battlefield)
            {
              permanent.RemoveSummoningSickness();
              permanent.Untap();
            }

            _players.Active.CanPlayLands = true;
          },
        nextStep: () => Step.Upkeep);

      CreateStep(
        Step.Upkeep,
        nextStep: () => Step.Draw);

      CreateStep(
        Step.Draw,
        first: () =>
          {
            if (_turnInfo.TurnCount != 1)
            {
              _players.Active.DrawCard();
            }
          },
        nextStep: () => Step.FirstMain);

      CreateStep(
        Step.FirstMain,
        nextStep: () => Step.BeginningOfCombat
        );

      CreateStep(
        Step.BeginningOfCombat,
        nextStep: () => Step.DeclareAttackers
        );

      CreateStep(
        Step.DeclareAttackers,
        first: DeclareAttackers,
        nextStep: () => Step.DeclareBlockers
        );

      CreateStep(
        Step.DeclareBlockers,
        first: DeclareBlockers,
        second: SetDamageAssignmentOrder,
        nextStep: () => _combat.AnyCreaturesWithFirstStrike()
          ? Step.FirstStrikeCombatDamage
          : Step.CombatDamage);

      CreateStep(
        Step.FirstStrikeCombatDamage,
        first: () => _combat.AssignCombatDamage(_decisions, firstStrike: true),
        second: () => _combat.DealAssignedDamage(),
        third: () => _players.MoveDeadCreaturesToGraveyard(),
        nextStep: () => _combat.AnyCreaturesWithNormalStrike()
          ? Step.CombatDamage
          : Step.EndOfCombat);

      CreateStep(
        Step.CombatDamage,
        first: () => _combat.AssignCombatDamage(_decisions),
        second: () => _combat.DealAssignedDamage(),
        third: () => _players.MoveDeadCreaturesToGraveyard(),
        nextStep: () => Step.EndOfCombat);

      CreateStep(
        Step.EndOfCombat,
        first: () => _combat.RemoveAll(),
        nextStep: () => Step.SecondMain);

      CreateStep(
        Step.SecondMain,
        nextStep: () => Step.EndOfTurn);

      CreateStep(
        Step.EndOfTurn,
        nextStep: () => Step.CleanUp);

      CreateStep(
        Step.CleanUp,
        getPriority: false,
        first: () =>
          {
            _players.RemoveDamageFromPermanents();
            _players.RemoveRegenerationFromPermanents();

            DiscardToMaximumHandSize();
            Publish(new EndOfTurn());
          },
        second: () =>
          {
            _players.ChangeActivePlayer();
            _turnInfo.TurnCount++;
          },
        nextStep: () => Step.Untap);
    }

    private void Publish<T>(T message)
    {
      _publisher.Publish(message);
    }

    private Player RollDice()
    {
      var dice1 = new Dice();
      var dice2 = new Dice();

      while (true)
      {
        dice1.Roll();
        dice2.Roll();

        if (dice1.LastResult != dice2.LastResult)
          break;
      }

      return dice1.LastResult > dice2.LastResult
        ? _players.Player1
        : _players.Player2;
    }

    private void SelectStartingPlayer()
    {
      var winner = _looser ?? RollDice();
      _decisions.EnqueueSelectStartingPlayer(winner);
    }

    private void SetDamageAssignmentOrder()
    {
      _combat.SetDamageAssignmentOrder(_decisions);
    }

    private void ShuffleLibraries()
    {
      foreach (var player in _players)
      {
        player.ShuffleLibrary();
      }
    }

    private void TakeMulligans()
    {
      var starting = _players.Starting;
      var nonStarting = _players.GetOpponent(starting);

      _decisions.EnqueueTakeMulligan(starting);
      _decisions.EnqueueTakeMulligan(nonStarting);
    }


    private class StepDefinition
    {
      private readonly Action _first;
      private readonly Func<Step> _next;
      private readonly Action _second;
      private readonly Action _third;

      public StepDefinition(
        Step step,
        Func<Step> nextStep,
        bool getPriority = true,
        Action first = null,
        Action second = null,
        Action third = null)
      {
        Step = step;
        GetPriority = getPriority;

        _next = nextStep;
        _third = third ?? delegate { };
        _first = first ?? delegate { };
        _second = second ?? delegate { };
      }

      public bool GetPriority { get; private set; }

      public Step Next
      {
        get { return _next(); }
      }

      public Step Step { get; private set; }

      public void First()
      {
        _first();
      }

      public void Second()
      {
        _second();
      }

      public void Third()
      {
        _third();
      }

      public override string ToString()
      {
        return Step.ToString();
      }
    }

    private class StepState
    {
      private readonly State _id;
      private readonly Func<State> _next;
      private readonly Action _proc;

      public StepState(State id, Action proc, Func<State> next)
      {
        _proc = proc;
        _next = next;
        _id = id;
      }


      public State Next
      {
        get { return _next(); }
      }

      public void Execute()
      {
        _proc();
      }

      public override string ToString()
      {
        return _id.ToString();
      }
    }
  }

  public enum State
  {
    Begin,
    First,
    Second,
    Third,
    Start,
    Active,
    Passive,
    BeginResolve,
    After,
    End,
    FinishResolve
  }
}