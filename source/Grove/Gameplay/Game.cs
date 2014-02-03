namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Artifical;
  using Costs;
  using DamageHandling;
  using Decisions;
  using Decisions.Scenario;
  using Infrastructure;
  using ManaHandling;
  using Modifiers;
  using Persistance;
  using States;
  using Targeting;
  using Zones;

  [Copyable]
  public class Game : IModifiable
  {
    private readonly CostModifiers _costModifiers;
    private readonly DamagePreventions _damagePreventions;
    private readonly DamageRedirections _damageRedirections;
    private readonly DecisionQueue _decisionQueue;
    private readonly DecisionSystem _decisionSystem;
    private readonly TrackableList<IGameModifier> _modifiers = new TrackableList<IGameModifier>();
    private readonly Publisher _publisher;
    private readonly StateMachine _stateMachine;
    private readonly bool _wasLoaded;
    private readonly Trackable<bool> _wasStopped;
    private int _turnLimit = int.MaxValue;

    private Game() {}

    public Game(GameParameters p, CardFactory cardFactory, DecisionSystem decisionSystem)
    {
      ChangeTracker = new ChangeTracker();
      _publisher = new Publisher();
      CardFactory = cardFactory;
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

      _damagePreventions = new DamagePreventions();
      _damageRedirections = new DamageRedirections();
      _costModifiers = new CostModifiers();

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
              return (Turn.StateCount < p.SavedGame.StateCount - p.RollBack) || (_decisionQueue.Count > 0);
            }

            return Recorder.IsPlayback;
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
      }
    }

    public ChangeTracker ChangeTracker { get; private set; }
    public CardFactory CardFactory { get; private set; }
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

      _damageRedirections.Initialize(ChangeTracker);
      _damagePreventions.Initialize(ChangeTracker);
      _costModifiers.Initialize(ChangeTracker);
      _modifiers.Initialize(ChangeTracker);
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

    private bool ShouldContinue()
    {
      return !_wasStopped.Value && !IsFinished;
    }

    public void AddScenarioDecisions(IEnumerable<DecisionsForOneStep> prerecordedDecisions)
    {
      _decisionSystem.AddScenarioDecisions(prerecordedDecisions);
    }

    public IManaAmount GetActualCost(IManaAmount amount, ManaUsage usage, Card card)
    {
      return _costModifiers.GetActualCost(amount, usage, card);
    }

    public interface IFactory
    {
      Game Create(GameParameters p);
    }
  }
}