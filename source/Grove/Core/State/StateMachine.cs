namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Events;
  using Infrastructure;

  public class StateMachine : GameObject, ICopyContributor
  {
    private readonly Trackable<IDecisionHandler> _currentHandler = new Trackable<IDecisionHandler>();
    private Trackable<StateNode> _currentState = new Trackable<StateNode>();
    private Trackable<StepNode> _currentStep = new Trackable<StepNode>();
    private DecisionQueue _decisionQueue;
    private Player _previousLooser;
    private List<StepNode> _steps;

    private IDecisionHandler CurrentHandler { get { return _currentHandler.Value; } set { _currentHandler.Value = value; } }
    public bool WasPriorityPassed { get { return CurrentHandler.IsPass; } }
    public bool HasPendingDecisions { get { return _decisionQueue.Count > 0; } }

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

    public void Initialize(Game game, DecisionQueue decisionQueue)
    {
      Game = game;
      _decisionQueue = decisionQueue;

      _currentHandler.Initialize(ChangeTracker);
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

    public new void Enqueue(Decision decision)
    {
      _decisionQueue.Enqueue(decision);
    }

    private void DeclareAttackers()
    {
      Enqueue(new DeclareAttackers(
        Players.Attacking));
    }

    private void DeclareBlockers()
    {
      Enqueue(new DeclareBlockers(Players.Defending));
    }

    private void DiscardToMaximumHandSize()
    {
      var discardCount = Players.Active.NumberOfCardsAboveMaximumHandSize;

      Enqueue(new DiscardCards(
        Players.Active,
        p => p.Count = discardCount));
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
      var should = false;

      while (shouldContinue())
      {
        if (CurrentHandler == null || CurrentHandler.HasCompleted)
        {
          if (_decisionQueue.Count == 0)
          {
            should = true;
            break;
          }

          var decision = _decisionQueue.Dequeue();
          CurrentHandler = decision.CreateHandler(Game);          
        }

        // execute is called multiple times, during
        // search tree generation        
        CurrentHandler.Execute();        
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
                    Enqueue(new ChooseToUntap(
                      Players.Active,
                      permanent));
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
            () => Combat.DistributeCombatDamage(firstStrike: true),
            DealAssignedCombatDamage,
            () => CheckAndMovePermanentsToGraveyard()
          },
        next: () => Combat.AnyCreaturesWithNormalStrike()
          ? combatDamage
          : endofCombat,
        getsPriority: true);

      combatDamage = NewStep(Step.CombatDamage,
        new Action[]
          {
            () => Combat.DistributeCombatDamage(),
            DealAssignedCombatDamage,
            () => CheckAndMovePermanentsToGraveyard()
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
                Publish(new EndOfTurnEvent());
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

    private void CheckAndMovePermanentsToGraveyard()
    {
      MovePermanentsToGraveyard(Players.Active);
      MovePermanentsToGraveyard(Players.Passive);
      
      CheckLegendaryRule();
      
      CheckPlaneswalkerRule(Players.Active);
      CheckPlaneswalkerRule(Players.Passive);
    }

    private void CheckPlaneswalkerRule(Player player)
    {
      /* This is a simplification of planeswalker rule -
         the active player - passive player order is 
         not respected (because decisions are always
         processed last.
         
       * If we want strict ordering we should implement
         this in 2 phases, first select everything
         then move to graveyard in active, passive order
      */

      var duplicatePlaneswalkers = player.Battlefield
        .Where(x => x.Is().Planeswalker)
        .GetDuplicates(x => x.Name)
        .ToArray();

      for (int i = 0; i < duplicatePlaneswalkers.Length; i+=2)
      {
        Enqueue(new SelectCards(player, p =>
          {
            p.MinCount = 1;
            p.MaxCount = 1;
            p.Zone = Zone.Battlefield;
            p.ValidCards = new List<Card> {duplicatePlaneswalkers[i], duplicatePlaneswalkers[i + 1]};
            p.SetValidator(c => p.ValidCards.Contains(c));
            p.Text = String.Format("{0}: Choose a planeswalker to sacrifice.", duplicatePlaneswalkers[i].Name);            
            
            p.SetChooseDecisionResults(candidates => 
              new ChosenCards(candidates.OrderBy(x => x.Loyality).First()));
            
            p.SetProcessDecisionResults(results => 
              results[0].Sacrifice());
          }));
      }
    }

    private void CheckLegendaryRule()
    {
      var duplicateLegends = Players
        .SelectMany(x => x.Battlefield.Legends)
        .GetDuplicates(card => card.Name).ToArray();

      foreach (var legend in duplicateLegends)
      {
        legend.Sacrifice();
      }
    }

    private void MovePermanentsToGraveyard(Player player)
    {
      var permanents = player.Battlefield.ToList();

      foreach (var permanent in permanents)
      {
        if (permanent.Is().Creature)
        {
          if (permanent.Toughness <= 0)
          {
            permanent.Sacrifice();            
          }

          else if (permanent.HasLeathalDamage || permanent.Life <= 0)
          {
            permanent.Destroy(allowToRegenerate: true);
          }
        }

        else if (permanent.Is().Planeswalker)
        {
          if (permanent.Loyality == 0)
          {
            permanent.Sacrifice();
          }
        }        
      }
    }

    private void DealAssignedCombatDamage()
    {
      Combat.DealAssignedDamage();

      if (Combat.AnyCreaturesWithFirstStrike() && Turn.Step == Step.FirstStrikeCombatDamage)
        Publish(new AssignedDamageDealtEvent(Turn.Step));

      if (Combat.AnyCreaturesWithNormalStrike() && Turn.Step == Step.CombatDamage)
        Publish(new AssignedDamageDealtEvent(Turn.Step));
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
      Enqueue(new SelectStartingPlayer(winner));
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

      Enqueue(new TakeMulligan(starting));
      Enqueue(new TakeMulligan(nonStarting));
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
                priorityFinishResolve = null,
                pushTriggeredActive = null,
                pushTriggeredPassive = null;
      

      if (getsPriority)
      {
        priorityActiveStart = new StateNode(
          100,
          () => Enqueue(new PlaySpellOrAbility(Players.Active)),
          () =>
            {
              if (WasPriorityPassed)
              {
                return priorityPassive;
              }

              return pushTriggeredActive;
            }
          );

        nodes.Add(priorityActiveStart);

        priorityActive = new StateNode(
          101,
          () => Enqueue(new PlaySpellOrAbility(Players.Active)),
          () =>
            {
              if (WasPriorityPassed)
              {
                return priorityBeginResolve;
              }

              return pushTriggeredActive;
            });

        nodes.Add(priorityActive);

        priorityPassive = new StateNode(
          102,
          () => Enqueue(new PlaySpellOrAbility(Players.Passive)),
          () =>
            {
              if (WasPriorityPassed)
              {
                return Stack.IsEmpty
                  ? priorityAfter
                  : priorityBeginResolve;
              }

              return pushTriggeredPassive;
            }
          );

        nodes.Add(priorityPassive);

        priorityPassiveStart = new StateNode(
          103,
          () => Enqueue(new PlaySpellOrAbility(Players.Passive)),
          () =>
            {
              if (WasPriorityPassed)
              {
                return priorityActive;
              }

              return pushTriggeredPassive;
            }
          );

        nodes.Add(priorityPassiveStart);        


        priorityBeginResolve = new StateNode(
          104,
          () => Stack.BeginResolve(),
          () => priorityFinishResolve);

        nodes.Add(priorityBeginResolve);

        priorityFinishResolve = new StateNode(
          105,
          () => Stack.FinishResolve(),
          () =>
            {
              // this executes after all decisions are processed
              // e.g after creature type for engineered plague has been
              // selected

              CheckAndMovePermanentsToGraveyard();
              return pushTriggeredActive;
            });

        nodes.Add(priorityFinishResolve);
      }

      priorityAfter = new StateNode(
        106,
        () =>
          {
            Players.Player1.EmptyManaPool();
            Players.Player2.EmptyManaPool();

            Publish(new StepFinishedEvent(Turn.Step));
          },
        () => null);

      nodes.Add(priorityAfter);

      pushTriggeredActive = new StateNode(
          107,
          () => Stack.PushTriggered(),
          () =>
          {
            if (Stack.HasTriggered)
            {
              return pushTriggeredActive;
            }

            return priorityActiveStart;
          });
      
      nodes.Add(pushTriggeredActive);

      pushTriggeredPassive = new StateNode(
          108,
          () => Stack.PushTriggered(),
          () =>
          {
            if (Stack.HasTriggered)
            {
              return pushTriggeredPassive;
            }

            return priorityPassiveStart;
          });

      nodes.Add(pushTriggeredPassive);
      
      var nextNode = getsPriority ? pushTriggeredActive : priorityAfter;

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
        () => Publish(new StepStartedEvent(Turn.Step)),
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