namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;
  using Artifical;
  using Decisions;
  using Decisions.Scenario;
  using Infrastructure;
  using Persistance;
  using States;
  using Zones;

  [Copyable]
  public class Game
  {
    private readonly DecisionQueue _decisionQueue;
    private readonly DecisionSystem _decisionSystem;
    private readonly Publisher _publisher;
    private readonly StateMachine _stateMachine;
    private readonly Trackable<bool> _wasStopped;
    private int _turnLimit = int.MaxValue;
    private readonly bool _wasLoaded;

    private Game() {}

    public Game(GameParameters p, CardsDatabase cardsDatabase, DecisionSystem decisionSystem)
    {
      ChangeTracker = new ChangeTracker();
      _publisher = new Publisher();
      CardsDatabase = cardsDatabase;
      Stack = new Stack();
      Turn = new TurnInfo();
      _wasStopped = new Trackable<bool>();
      Combat = new Combat();
      Ai = new SearchRunner(p.SearchParameters, this);
      _decisionSystem = decisionSystem;
      _decisionQueue = new DecisionQueue();
      _stateMachine = new StateMachine(_decisionQueue);

      var player1 = new Player(p.Player1, p.Player1Controller);
      var player2 = new Player(p.Player2, p.Player2Controller);
      Players = new Players(player1, player2);

      if (p.IsSavedGame)
      {
        Random = new RandomGenerator(p.SavedGame.RandomSeed);
        Recorder = new GameRecorder(this, p.SavedGame.Decisions);
      }
      else
      {
        Random = new RandomGenerator();
        Recorder = new GameRecorder(this);
      }

      Initialize();

      if (p.IsSavedGame)
      {
        _stateMachine.Start(() => Turn.StateCount < p.SavedGame.StateCount, skipPreGame: false);
        _wasLoaded = true;
      }
    }

    public ChangeTracker ChangeTracker { get; private set; }
    public CardsDatabase CardsDatabase { get; private set; }
    public bool WasStopped { get { return _wasStopped.Value; } }
    public Combat Combat { get; private set; }
    public bool IsFinished { get { return Players.AnyHasLost() || _turnLimit < Turn.TurnCount; } }
    public Players Players { get; private set; }
    public int Score { get { return Players.Score; } }
    public Stack Stack { get; private set; }
    public TurnInfo Turn { get; private set; }
    public SearchRunner Ai { get; private set; }
    public RandomGenerator Random { get; private set; }
    public GameRecorder Recorder { get; private set; }

    private void Initialize()
    {
      _publisher.Initialize(ChangeTracker);

      Stack.Initialize(this);
      Turn.Initialize(this);
      _wasStopped.Initialize(ChangeTracker);
      Combat.Initialize(this);
      _decisionQueue.Initialize(this);
      _stateMachine.Initialize(this);
      Players.Initialize(this);
    }

    public void Enqueue<TDecision>(Player controller, Action<TDecision> init = null)
      where TDecision : class, IDecision
    {
      init = init ?? delegate { };

      var decision = _decisionSystem.Create(controller, init, this);
      _decisionQueue.Enqueue(decision);
    }

    public SavedGame Save()
    {
      return Recorder.SaveGame();
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

    public int CalculateHash()
    {
      var calc = new HashCalculator();

      return HashCalculator.Combine(
        calc.Calculate(Players),
        calc.Calculate(Stack),
        calc.Calculate(Turn),
        calc.Calculate(Combat));
    }

    public void RollbackToSnapshot(object snaphost)
    {
      ChangeTracker.RollbackToSnapshot((Snapshot) snaphost);
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

      try
      {
        if (_wasLoaded)
        {
          _stateMachine.Resume(ShouldContinue);
        }
        else
        {
          _stateMachine.Start(ShouldContinue, skipPreGame, looser);  
        }        
      }
      catch (Exception)
      {
        DumpCrashReport();
        throw;
      }
    }

    private void DumpCrashReport()
    {
      var formatter = new BinaryFormatter();
      
      var filename = String.Format("crash-report-{0}.report", Guid.NewGuid());
      using (var file = new FileStream(filename, FileMode.Create))
      {
        var savedGame = Save();
        formatter.Serialize(file,savedGame);
      }
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

    public interface IFactory
    {
      Game Create(GameParameters p);
    }
  }
}