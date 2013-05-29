namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Decisions;
  using Decisions.Scenario;
  using Infrastructure;
  using Misc;
  using States;
  using Zones;  
  
  [Copyable]
  public class Game
  {
    private DecisionQueue _decisionQueue;
    private DecisionSystem _decisionSystem;
    private Publisher _publisher;
    private StateMachine _stateMachine;
    private int _turnLimit = int.MaxValue;
    private Trackable<bool> _wasStopped;

    private Game() {}

    public ChangeTracker ChangeTracker { get; private set; }
    public CardsDatabase CardsDatabase { get; private set; }
    public bool WasStopped { get { return _wasStopped.Value; } }
    public Combat Combat { get; private set; }
    public bool IsFinished { get { return Players.AnyHasLost() || _turnLimit < Turn.TurnCount; } }
    public Players Players { get; set; }
    public int Score { get { return Players.Score; } }
    public Stack Stack { get; private set; }
    public TurnInfo Turn { get; private set; }
    public SearchRunner Ai { get; private set; }    

    public static Game New(string yourName, string opponentsName, Deck humanDeck, Deck cpuDeck,
      CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      var searchParameters = new SearchParameters(40, 2, enableMultithreading: true);
      var game = CreateGame(searchParameters, cardsDatabase, decisionSystem);

      var player1 = new Player(yourName, "player1.png", ControllerType.Human, humanDeck);
      var player2 = new Player(opponentsName, "player2.png", ControllerType.Machine, cpuDeck);
      game.Players = new Players(player1, player2);            

      return game.Initialize();
    }

    private Game Initialize()
    {
      _publisher.Initialize(ChangeTracker);
      Stack.Initialize(this);
      Turn.Initialize(this);
      _wasStopped.Initialize(ChangeTracker);
      Combat.Initialize(this);
      _decisionQueue.Initialize(this);
      _stateMachine.Initialize(this);
      Players.Initialize(this);

      return this;
    }

    private static Game CreateGame(SearchParameters searchParameters, CardsDatabase cardsDatabase,
      DecisionSystem decisionSystem)
    {
      var game = new Game();

      game.ChangeTracker = new ChangeTracker();
      game._publisher = new Publisher();
      game.CardsDatabase = cardsDatabase;
      game.Stack = new Stack();
      game.Turn = new TurnInfo();
      game._wasStopped = new Trackable<bool>();
      game.Combat = new Combat();
      game.Ai = new SearchRunner(searchParameters, game);
      game._decisionSystem = decisionSystem;
      game._decisionQueue = new DecisionQueue();
      game._stateMachine = new StateMachine(game._decisionQueue);

      return game;
    }

    public void Enqueue<TDecision>(Player controller, Action<TDecision> init = null)
      where TDecision : class, IDecision
    {
      init = init ?? delegate { };

      var decision = _decisionSystem.Create(controller, init, this);
      _decisionQueue.Enqueue(decision);
    }

    public void Publish<TMessage>(TMessage message)
    {
      _publisher.Publish(message);
    }

    public void Unsubscribe(object instance)
    {
      _publisher.Unsubscribe(instance);
    }

    public void Subscribe(object instance)
    {
      _publisher.Subscribe(instance);
    }

    public static Game NewSimulation(Deck deck1, Deck deck2, int maxSearchDepth, int maxTargetCount,
      CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      var searchParameters = new SearchParameters(maxSearchDepth, maxTargetCount, enableMultithreading: false);
      var game = CreateGame(searchParameters, cardsDatabase, decisionSystem);

      var player1 = new Player("Player1", "player1.png", ControllerType.Machine, deck1);
      var player2 = new Player("Player2", "player2.png", ControllerType.Machine, deck2);
      game.Players = new Players(player1, player2);

      return game.Initialize();
    }

    public int CalculateHash()
    {
      var calc = new HashCalculator();

      return HashCalculator.Combine(
        calc.Calculate(Players),
        calc.Calculate(Stack),
        calc.Calculate(Turn),
        calc.Calculate(Combat));
    }

    public void Restore(object snaphost)
    {
      ChangeTracker.Restore((Snapshot) snaphost);
    }

    public object CreateSnapshot()
    {
      return ChangeTracker.CreateSnapshot();
    }

    public void Simulate(int maxStepCount)
    {
      _stateMachine.Resume(() => Turn.StepCount < maxStepCount && ShouldContinue());
    }

    public void Start(int numOfTurns = int.MaxValue, bool skipPreGame = false, Player looser = null)
    {
      _turnLimit = numOfTurns;

      _stateMachine.Start(ShouldContinue, skipPreGame, looser);
    }

    public void Stop()
    {
      _wasStopped.Value = true;
    }

    public override string ToString()
    {
      return Turn.ToString();
    }

    private bool ShouldContinue()
    {
      return !_wasStopped.Value && !IsFinished;
    }

    public void AddScenarioDecisions(IEnumerable<DecisionsForOneStep> prerecordedDecisions)
    {
      _decisionSystem.AddScenarioDecisions(prerecordedDecisions);
    }

    public static Game NewScenario(ControllerType player1Controller, ControllerType player2Controller,
      CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      var searchParameters = new SearchParameters(40, 2, enableMultithreading: true);
      var game = CreateGame(searchParameters, cardsDatabase, decisionSystem);

      var player1 = new Player("Player1", "player1.png", player1Controller, Deck.CreateUncastable());
      var player2 = new Player("Player2", "player2.png", player2Controller, Deck.CreateUncastable());
      game.Players = new Players(player1, player2);
      game.Initialize();

      game.Players.Starting = game.Players.Player1;
      return game;
    }
  }
}