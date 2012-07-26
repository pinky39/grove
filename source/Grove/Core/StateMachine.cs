namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Controllers;
  using Infrastructure;
  using log4net;
  using Messages;

  [Copyable]
  public class StateMachine : ICopyContributor
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (StateMachine));
    private readonly DecisionQueue _decisionQueue;
    private Trackable<IDecision> _curentDecision;
    private Trackable<int> _passesCount;
    private Game _game;
    private Player _looser;
    private Dictionary<State, StepState> _states;
    private Dictionary<Step, StepDefinition> _steps;

    private StateMachine() {}

    public StateMachine(DecisionQueue decisionQueue)
    {
      _decisionQueue = decisionQueue;
    }

    private IDecision CurrentDecision { get { return _curentDecision.Value; } set { _curentDecision.Value = value; } }

    private State State
    {
      get { return _game.Turn.State; }
      set
      {
        _game.Turn.State = value;
        Log.DebugFormat("State: {0}", value);
      }
    }

    private Step Step { get { return _game.Turn.Step; } set { _game.Turn.Step = value; } }

    private bool WasPriorityPassed { get { return CurrentDecision.WasPriorityPassed; } }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      InitializeStepStates();
      InitializeSteps();
    }

    public StateMachine Init(Game game)
    {
      _game = game;
      _curentDecision = new Trackable<IDecision>(_game.ChangeTracker);
      _passesCount = new Trackable<int>(_game.ChangeTracker);

      InitializeStepStates();
      InitializeSteps();
      return this;
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
      _game.Decisions.Enqueue<DeclareAttackers>(
        controller: _game.Players.Attacking);
    }

    private void DeclareBlockers()
    {
      _game.Decisions.Enqueue<DeclareBlockers>(
        controller: _game.Players.Defending);
    }

    private void DiscardToMaximumHandSize()
    {
      var activePlayer = _game.Players.Active;

      _game.Decisions.Enqueue<DiscardCards>(
        controller: _game.Players.Active,
        init: p => p.Count = activePlayer.NumberOfCardsAboveMaximumHandSize);
    }

    private void DrawStartingHands()
    {
      foreach (var player in _game.Players)
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
          if (_decisionQueue.Count == 0)
            break;

          CurrentDecision = _decisionQueue.Dequeue();
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
        proc: () => _game.Decisions.Enqueue<PlaySpellOrAbility>(_game.Players.Active),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              _passesCount.Value++;
              return State.Passive;
            }
            
            return State.Active;
          });

      CreateState(
        name: State.Active,
        proc: () => _game.Decisions.Enqueue<PlaySpellOrAbility>(_game.Players.Active),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              _passesCount.Value++;
              
              if (_passesCount.Value == 2)
              {
                _passesCount.Value = 0;
                
                return _game.Stack.IsEmpty 
                  ? State.After 
                  : State.BeginResolve;
              }

              return State.Passive;                
            }

            _passesCount.Value = 0;
            return State.Active;
          });

      CreateState(
        name: State.Passive,
        proc: () => _game.Decisions.Enqueue<PlaySpellOrAbility>(_game.Players.Passive),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              _passesCount.Value++;
              
              if (_passesCount.Value == 2)
              {
                _passesCount.Value = 0;
                
                return _game.Stack.IsEmpty 
                  ? State.After 
                  : State.BeginResolve;
              }

              return State.Active;                                            
            }

            _passesCount.Value = 0;
            return State.Passive;
          });

      CreateState(
        name: State.BeginResolve,
        proc: () => _game.Stack.Resolve(),
        next: () => State.FinishResolve);

      CreateState(
        name: State.FinishResolve,
        proc: () =>
          {
            var effect = _game.Stack.LastResolved;
            if (effect != null)
            {              
              effect.FinishResolve();
              _game.Players.MoveDeadCreaturesToGraveyard();
            }
          },
        next: () => State.Start);

      CreateState(
        name: State.After,
        proc: () =>
          {
            _game.Players.Player1.EmptyManaPool();
            _game.Players.Player2.EmptyManaPool();

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
        nextStep: () => _game.Players.AnotherMulliganRound ? Step.Mulligan : Step.Untap);

      CreateStep(
        Step.Untap,
        getPriority: false,
        first: () => Publish(new TurnStarted {TurnCount = _game.Turn.TurnCount}),
        second: () =>
          {
            foreach (var permanent in _game.Players.Active.Battlefield)
            {
              permanent.HasSummoningSickness = false;
              
              if (permanent.MayChooseNotToUntapDuringUntapStep)
              {
                var permanentCopy = permanent;
                _game.Decisions.Enqueue<ChooseToUntap>(
                  controller: _game.Players.Active, 
                  init: p => p.Permanent = permanentCopy);
              }
              else
              {
                permanent.Untap();
              }
            }

            _game.Players.Active.CanPlayLands = true;
          },
        nextStep: () => Step.Upkeep);

      CreateStep(
        Step.Upkeep,   
        nextStep: () => Step.Draw);

      CreateStep(
        Step.Draw,
        first: () =>
          {
            if (_game.Turn.TurnCount != 1)
            {
              _game.Players.Active.DrawCard();
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
        nextStep: () => _game.Combat.AnyCreaturesWithFirstStrike()
          ? Step.FirstStrikeCombatDamage
          : Step.CombatDamage);

      CreateStep(
        Step.FirstStrikeCombatDamage,
        first: () => _game.Combat.AssignCombatDamage(_game.Decisions, firstStrike: true),
        second: DealAssignedCombatDamage,
        third: () => _game.Players.MoveDeadCreaturesToGraveyard(),
        nextStep: () => _game.Combat.AnyCreaturesWithNormalStrike()
          ? Step.CombatDamage
          : Step.EndOfCombat);

      CreateStep(
        Step.CombatDamage,
        first: () => _game.Combat.AssignCombatDamage(_game.Decisions),
        second: DealAssignedCombatDamage,
        third: () => _game.Players.MoveDeadCreaturesToGraveyard(),
        nextStep: () => Step.EndOfCombat);

      CreateStep(
        Step.EndOfCombat,
        first: () => _game.Combat.RemoveAll(),
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
            _game.Players.RemoveDamageFromPermanents();
            _game.Players.RemoveRegenerationFromPermanents();

            DiscardToMaximumHandSize();
            Publish(new EndOfTurn());
          },
        second: () =>
          {
            _game.Players.ChangeActivePlayer();
            _game.Turn.TurnCount++;
          },
        nextStep: () => Step.Untap);
    }

    private void DealAssignedCombatDamage()
    {
      _game.Combat.DealAssignedDamage();

      if (_game.Combat.AnyCreaturesWithFirstStrike() && Step == Step.FirstStrikeCombatDamage)
        Publish(new AssignedCombatDamageWasDealt(Step));

      if (_game.Combat.AnyCreaturesWithNormalStrike() && Step == Step.CombatDamage)
        Publish(new AssignedCombatDamageWasDealt(Step));
    }

    private void Publish<T>(T message)
    {
      _game.Publisher.Publish(message);
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
        ? _game.Players.Player1
        : _game.Players.Player2;
    }

    private void SelectStartingPlayer()
    {
      var winner = _looser ?? RollDice();
      _game.Decisions.Enqueue<SelectStartingPlayer>(winner);
    }

    private void SetDamageAssignmentOrder()
    {
      _game.Combat.SetDamageAssignmentOrder(_game.Decisions);
    }

    private void ShuffleLibraries()
    {
      foreach (var player in _game.Players)
      {
        player.ShuffleLibrary();
      }
    }

    private void TakeMulligans()
    {
      var starting = _game.Players.Starting;
      var nonStarting = _game.Players.GetOpponent(starting);

      _game.Decisions.Enqueue<TakeMulligan>(starting);
      _game.Decisions.Enqueue<TakeMulligan>(nonStarting);
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

      public Step Next { get { return _next(); } }

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


      public State Next { get { return _next(); } }

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