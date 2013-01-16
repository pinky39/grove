namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Decisions;
  using Decisions.Scenario;
  using Infrastructure;
  using Targeting;
  using Zones;

  [Copyable]
  public class Game
  {
    private DecisionQueue _decisionQueue;
    private DecisionSystem _decisionSystem;
    private Publisher _publisher;
    private Search _search;
    private StateMachine _stateMachine;
    private TurnInfo _turnInfo;
    private Trackable<bool> _wasStopped;

    private Game() {}

    public ChangeTracker ChangeTracker { get; private set; }
    public CardDatabase CardDatabase { get; private set; }
    public bool WasStopped { get { return _wasStopped.Value; } }
    public Combat Combat { get; private set; }
    public bool IsFinished { get { return Players.AnyHasLost(); } }
    public Players Players { get; set; }
    public int Score { get { return Players.Score; } }
    public Stack Stack { get; private set; }
    public TurnInfo Turn { get { return _turnInfo; } }
    public Search Search { get { return _search; } }

    public static Game New(IEnumerable<string> humanDeck, IEnumerable<string> cpuDeck,
      CardDatabase cardDatabase, DecisionSystem decisionSystem)
    {
      var game = CreateGame(cardDatabase, decisionSystem);

      game.Players = new Players(
        player1Name: "You",
        player1Type: ControllerType.Human,
        player1Deck: humanDeck,
        player2Name: "Cpu",
        player2Type: ControllerType.Machine,
        player2Deck: cpuDeck,
        game: game);

      return game;
    }

    private static Game CreateGame(CardDatabase cardDatabase, DecisionSystem decisionSystem)
    {
      var game = new Game();

      game.ChangeTracker = new ChangeTracker();
      game._publisher = new Publisher(game.ChangeTracker);
      game.CardDatabase = cardDatabase;
      game.Stack = new Stack(game.ChangeTracker);
      game._turnInfo = new TurnInfo(game.ChangeTracker);
      game._wasStopped = new Trackable<bool>(game.ChangeTracker);
      game.Combat = new Combat(game);
      game._search = new Search();
      game._decisionSystem = decisionSystem;
      game._decisionQueue = new DecisionQueue(game.ChangeTracker);
      game._stateMachine = new StateMachine(game, game._decisionQueue);

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

    public static Game NewSimulation(IEnumerable<string> deck1, IEnumerable<string> deck2, CardDatabase cardDatabase,
      DecisionSystem decisionSystem)
    {
      var game = CreateGame(cardDatabase, decisionSystem);

      game.Players = new Players(
        player1Name: "Player1",
        player1Type: ControllerType.Machine,
        player1Deck: deck1,
        player2Name: "Player2",
        player2Type: ControllerType.Machine,
        player2Deck: deck2,
        game: game);

      return game;
    }

    public IEnumerable<ITarget> GenerateTargets(Func<Zone, Player, bool> zoneFilter)
    {
      foreach (var target in Players.SelectMany(p => p.GetTargets(zoneFilter)))
      {
        yield return target;
      }

      foreach (var target in Stack.GenerateTargets(zoneFilter))
      {
        yield return target;
      }
    }

    public int CalculateHash()
    {
      var calc = new HashCalculator();

      return HashCalculator.Combine(
        calc.Calculate(Players),
        calc.Calculate(Stack),
        calc.Calculate(Turn),
        calc.Calculate(Combat)
        );
    }

    public void Restore(object snaphost)
    {
      ChangeTracker.Restore((Snapshot) snaphost);
    }

    public object Save()
    {
      return ChangeTracker.CreateSnapshot();
    }

    public void Simulate()
    {
      _stateMachine.Resume(ShouldContinue);
    }

    public void Start(int numOfTurns = int.MaxValue, bool skipPreGame = false, Player looser = null)
    {
      _stateMachine.Start(() => ShouldContinue() &&
        numOfTurns >= Turn.TurnCount, skipPreGame, looser);
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
      CardDatabase cardDatabase, DecisionSystem decisionSystem)
    {
      var game = CreateGame(cardDatabase, decisionSystem);

      game.Players = new Players(
        player1Name: "Player1",
        player1Type: player1Controller,
        player1Deck: DummyDeck(),
        player2Name: "Player2",
        player2Type: player2Controller,
        player2Deck: DummyDeck(),
        game: game);


      game.Players.Starting = game.Players.Player1;

      return game;
    }

    private static IEnumerable<string> DummyDeck()
    {
      return Enumerable.Repeat("Uncastable", 60);
    }
  }
}