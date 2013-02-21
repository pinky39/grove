namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Decisions;
  using Infrastructure;
  using log4net;
  using Messages;

  public class StateMachine : GameObject, ICopyContributor
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (StateMachine));
    private readonly Trackable<IDecision> _curentDecision = new Trackable<IDecision>();
    private readonly DecisionQueue _decisionQueue;
    private readonly Trackable<int> _passesCount = new Trackable<int>();
    private Player _looser;
    private Dictionary<State, StepState> _states;
    private Dictionary<Step, StepDefinition> _steps;

    private StateMachine() {}

    public StateMachine(DecisionQueue decisionQueue)
    {
      _decisionQueue = decisionQueue;

      InitializeStepStates();
      InitializeSteps();
    }

    private IDecision CurrentDecision { get { return _curentDecision.Value; } set { _curentDecision.Value = value; } }

    private State State
    {
      get { return Turn.State; }
      set
      {
        Turn.State = value;
        Log.DebugFormat("State: {0}", value);
      }
    }

    private Step Step { get { return Turn.Step; } set { Turn.Step = value; } }

    private bool WasPriorityPassed { get { return CurrentDecision.WasPriorityPassed; } }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      InitializeStepStates();
      InitializeSteps();
    }

    public void Initialize(Game game)
    {
      Game = game;

      _curentDecision.Initialize(ChangeTracker);
      _passesCount.Initialize(ChangeTracker);
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
        proc: () => Enqueue<PlaySpellOrAbility>(Players.Active),
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
        proc: () => Enqueue<PlaySpellOrAbility>(Players.Active),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              _passesCount.Value++;

              if (_passesCount.Value == 2)
              {
                _passesCount.Value = 0;

                return Stack.IsEmpty
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
        proc: () => Enqueue<PlaySpellOrAbility>(Players.Passive),
        next: () =>
          {
            if (WasPriorityPassed)
            {
              _passesCount.Value++;

              if (_passesCount.Value == 2)
              {
                _passesCount.Value = 0;

                return Stack.IsEmpty
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
        proc: () => Stack.Resolve(),
        next: () => State.FinishResolve);

      CreateState(
        name: State.FinishResolve,
        proc: () =>
          {
            var effect = Stack.LastResolved;
            if (effect != null)
            {
              effect.FinishResolve();
              Players.MoveDeadCreaturesToGraveyard();
            }
          },
        next: () => State.Start);

      CreateState(
        name: State.After,
        proc: () =>
          {
            Players.Player1.EmptyManaPool();
            Players.Player2.EmptyManaPool();

            Publish(new StepFinished
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
        nextStep: () => Players.AnotherMulliganRound ? Step.Mulligan : Step.Untap);

      CreateStep(
        Step.Untap,
        getPriority: false,
        first: () => Publish(new TurnStarted {TurnCount = Turn.TurnCount}),
        second: () =>
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
          },
        nextStep: () => Step.Upkeep);

      CreateStep(
        Step.Upkeep,
        nextStep: () => Step.Draw);

      CreateStep(
        Step.Draw,
        first: () =>
          {
            if (Turn.TurnCount != 1)
            {
              Players.Active.DrawCard();
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
        nextStep: () => Combat.AnyCreaturesWithFirstStrike()
          ? Step.FirstStrikeCombatDamage
          : Step.CombatDamage);

      CreateStep(
        Step.FirstStrikeCombatDamage,
        first: () => Combat.AssignCombatDamage(firstStrike: true),
        second: DealAssignedCombatDamage,
        third: () => Players.MoveDeadCreaturesToGraveyard(),
        nextStep: () => Combat.AnyCreaturesWithNormalStrike()
          ? Step.CombatDamage
          : Step.EndOfCombat);

      CreateStep(
        Step.CombatDamage,
        first: () => Combat.AssignCombatDamage(),
        second: DealAssignedCombatDamage,
        third: () => Players.MoveDeadCreaturesToGraveyard(),
        nextStep: () => Step.EndOfCombat);

      CreateStep(
        Step.EndOfCombat,
        first: () => Combat.RemoveAll(),
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
            Players.RemoveDamageFromPermanents();
            Players.RemoveRegenerationFromPermanents();

            DiscardToMaximumHandSize();
            Publish(new EndOfTurn());
          },
        second: () =>
          {
            Players.ChangeActivePlayer();
            Turn.TurnCount++;
          },
        nextStep: () => Step.Untap);
    }

    private void DealAssignedCombatDamage()
    {
      Combat.DealAssignedDamage();

      if (Combat.AnyCreaturesWithFirstStrike() && Step == Step.FirstStrikeCombatDamage)
        Publish(new AssignedCombatDamageWasDealt(Step));

      if (Combat.AnyCreaturesWithNormalStrike() && Step == Step.CombatDamage)
        Publish(new AssignedCombatDamageWasDealt(Step));
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
        ? Players.Player1
        : Players.Player2;
    }

    private void SelectStartingPlayer()
    {
      var winner = _looser ?? RollDice();
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