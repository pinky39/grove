namespace Grove.Core.Decisions.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using log4net;
  using Results;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility, ISearchNode, IDecisionExecution
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (PlaySpellOrAbility));
    private DecisionExecutor _executor;
    private List<Playable> _playables;

    public PlaySpellOrAbility()
    {
      Result = DefaultResult();
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }        
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    public int ResultCount { get { return _playables.Count; } }

    public void GenerateChoices()
    {
      _playables = GeneratePlayables().ToList();
    }

    public void SetResult(int index)
    {
      Result = _playables[index];
      Log.DebugFormat("Move is {0}", _playables[index]);
    }

    public override void Init()
    {
      _executor = new DecisionExecutor(this, Game.ChangeTracker);
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

    private IEnumerable<Playable> GeneratePlayables()
    {
      if (Game.Stack.TopSpellOwner == Controller)
      {
        // if you own the top spell just pass so it resolves
        // you will get priority again when it resolves
        
        yield return new Pass();
        yield break;
      }

      foreach (var playable in new PlayableGenerator(Controller, Game))
      {
        yield return playable;
      }

      yield return new Pass();
    }

    public override string ToString()
    {
      return string.Format("{0}, {1} plays", Game.Turn.Step, Controller);
    }
  }
}