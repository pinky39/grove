namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using AI;
  using Costs;
  using Decisions;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Game : IModifiable
  {
    private readonly CostModifiers _costModifiers;
    private readonly DamagePreventions _damagePreventions;
    private readonly DamageRedirections _damageRedirections;
    private readonly NamedGameModifiers _namedGameModifiers;
    private readonly DecisionQueue _decisionQueue;
    private readonly TrackableList<IGameModifier> _modifiers = new TrackableList<IGameModifier>();
    private readonly Publisher _publisher;
    private readonly Scenario _scenario;
    private readonly StateMachine _stateMachine;
    private readonly bool _wasLoaded;
    private readonly Trackable<bool> _wasStopped;
    private int _turnLimit = int.MaxValue;

    private Game() {}

    public Game(GameParameters p)
    {
      ChangeTracker = new ChangeTracker();
      Stack = new Stack();
      Turn = new TurnInfo();
      Ai = new SearchRunner(p.SearchParameters, this);
      Combat = new Combat();

      _decisionQueue = new DecisionQueue();
      _publisher = new Publisher(changeTracker: ChangeTracker);
      _wasStopped = new Trackable<bool>();
      _stateMachine = new StateMachine();

      var player1 = new Player(p.Player1, p.Player1Controller);
      var player2 = new Player(p.Player2, p.Player2Controller);
      Players = new Players(player1, player2);

      _damagePreventions = new DamagePreventions();
      _damageRedirections = new DamageRedirections();
      _namedGameModifiers = new NamedGameModifiers();
      _costModifiers = new CostModifiers();
      _scenario = new Scenario(this);

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
        p.SavedGame.Decisions.Position = 0;

        Player looser = null;

        if (p.Looser != null)
        {
          looser = p.Looser == 0 ? player1 : player2;
        }

        _stateMachine.Start(() =>
          {
            if (p.RollBack > 0)
            {
              return (Turn.StateCount < p.SavedGame.StateCount - p.RollBack) || (_stateMachine.HasPendingDecisions);
            }

            return (Turn.StateCount < p.SavedGame.StateCount || Recorder.IsPlayback);
          }, skipPreGame: false, looser: looser);

        Recorder.DiscardUnloadedResults();
        _wasLoaded = true;
      }
    }

    private IEnumerable<IAcceptsGameModifier> ModifiableProperties
    {
      get
      {
        yield return _damagePreventions;
        yield return _damageRedirections;
        yield return _costModifiers;
        yield return _namedGameModifiers;
      }
    }

    public Scenario Scenario { get { return _scenario; } }
    public ChangeTracker ChangeTracker { get; private set; }
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

    public void RemoveModifier(IModifier modifier)
    {
      RemoveModifier((IGameModifier) modifier);
    }

    public void AddModifier(IGameModifier modifier, ModifierParameters p)
    {
      p.Owner = this;

      _modifiers.Add(modifier);
      modifier.Initialize(p, this);
      modifier.Activate();

      foreach (var modifiableProperty in ModifiableProperties)
      {
        modifiableProperty.Accept(modifier);
      }
    }

    public void RemoveModifier(IGameModifier modifier)
    {
      _modifiers.Remove(modifier);
      modifier.Dispose();
    }

    public int PreventDamage(PreventDamageParameters p)
    {
      return _damagePreventions.PreventDamage(p);
    }

    public int PreventLifeloss(int amount, Player player, bool queryOnly)
    {
      return _damagePreventions.PreventLifeloss(amount, player, queryOnly);
    }

    public bool RedirectDamage(Damage damage, ITarget target)
    {
      return _damageRedirections.RedirectDamage(damage, target);
    }

    public INamedGameModifiers Has()
    {
      return _namedGameModifiers;
    }

    public void Enqueue(Decision decision)
    {
      _stateMachine.Enqueue(decision);
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
        calc.Calculate(Combat),
        calc.Calculate(_damagePreventions),
        calc.Calculate(_damageRedirections),
        calc.Calculate(_decisionQueue));
    }

    public void RollbackToSnapshot(object snaphost)
    {
      ChangeTracker.RollbackToSnapshot((Snapshot) snaphost);
    }

    public object CreateSnapshot()
    {
      return ChangeTracker.CreateSnapshot();
    }

    public void Simulate(Func<bool> shouldContinue)
    {
      _stateMachine.Resume(shouldContinue);
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
        WriteDebugReport();
        throw;
      }
    }

    public void WriteDebugReport(string filename = null)
    {
      var header = new SaveFileHeader {Description = "Debug information to reproduce a bug which caused the error."};
      var savedGame = Save();

      filename = filename ?? String.Format("debug-report-{0}.report", Guid.NewGuid());
      using (var stream = new FileStream(filename, FileMode.Create))
      {
        SavedGames.WriteToStream(header, savedGame, stream);
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

    public IManaAmount GetActualCost(IManaAmount amount, ManaUsage usage, Card card)
    {
      return _costModifiers.GetActualCost(amount, usage, card);
    }

    private void Initialize()
    {
      Stack.Initialize(this);
      Turn.Initialize(this);
      _wasStopped.Initialize(ChangeTracker);
      Combat.Initialize(this);
      _decisionQueue.Initialize(this);
      _stateMachine.Initialize(this, _decisionQueue);
      Players.Initialize(this);

      _damageRedirections.Initialize(ChangeTracker);
      _damagePreventions.Initialize(ChangeTracker);
      _costModifiers.Initialize(ChangeTracker);
      _namedGameModifiers.Initialize(this);
      _modifiers.Initialize(ChangeTracker);
    }

    private bool ShouldContinue()
    {
      return !_wasStopped.Value && !IsFinished;
    }
  }
}