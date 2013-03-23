namespace Grove.Core.Decisions.Machine
{
  using System.Collections.Generic;
  using Ai;
  using log4net;
  using Results;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility, ISearchNode, IDecisionExecution
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (PlaySpellOrAbility));
    private readonly DecisionExecutor _executor;
    private List<Playable> _playables;

    public PlaySpellOrAbility()
    {
      Result = DefaultResult();
      _executor = new DecisionExecutor(this);
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    Game ISearchNode.Game { get { return Game; } }

    public int ResultCount { get { return _playables.Count; } }

    public void GenerateChoices()
    {
      _playables = GeneratePlayables();

      // consider passing priority every time
      _playables.Add(new Pass());
    }

    public void SetResult(int index)
    {
      Result = _playables[index];
      Log.DebugFormat("Move is {0}", _playables[index]);
    }

    public override void Initialize(Player controller, Game game)
    {
      base.Initialize(controller, game);
      _executor.Initialize(game.ChangeTracker);
    }

    public override void Execute()
    {
      _executor.Execute();
    }

    protected override void ExecuteQuery()
    {
      Game.Search.SetBestResult(this);
    }

    private static ChosenPlayable DefaultResult()
    {
      // this is used when search is stoped when search depth is 
      // reached      
      return new ChosenPlayable {Playable = new Pass()};
    }

    private List<Playable> GeneratePlayables()
    {
      if (Game.Stack.TopSpellOwner == Controller)
      {
        // if you own the top spell just pass so it resolves
        // you will get priority again when it resolves
        return new List<Playable>();
      }

      return new PlayableGenerator(Controller, Game).GetPlayables();
    }

    public override string ToString()
    {
      return string.Format("{0}, {1} plays", Game.Turn.Step, Controller);
    }
  }
}