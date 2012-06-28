namespace Grove.Core.Controllers.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;
  using log4net;
  using Results;

  public class PlaySpellOrAbility : Controllers.PlaySpellOrAbility, ISearchNode, IDecisionExecution
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof(PlaySpellOrAbility));
    private readonly DecisionExecutor _executor;
    private List<Playable> _playables;

    private PlaySpellOrAbility() {}

    public PlaySpellOrAbility(ChangeTracker changeTracker)
    {
      _executor = new DecisionExecutor(this, changeTracker);
      Result = DefaultResult();
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }

    public CastRestrictions Restrictions { get; set; }
    public Search Search { get; set; }

    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    public Game Game { get; set; }

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

    public override void Execute()
    {
      _executor.Execute();
    }

    protected override void ExecuteQuery()
    {
      Search.SetBestResult(this);
    }

    private static ChosenPlayable DefaultResult()
    {
      // this is used when search is stoped when search depth is 
      // reached      
      return new ChosenPlayable{Playable = new Pass()};
    }

    private IEnumerable<Playable> GeneratePlayables()
    {                                                
            
      if (!Restrictions.IsPlayRestrictedFor(Player))
      {
        foreach (var playable in new PlayableGenerator(Player, Game))
        {
          yield return playable;
        }
      }                                                

      yield return new Pass();
    }

    public override string ToString()
    {
      return string.Format("{0}, {1} plays", Game.Turn.Step, Player);
    }
  }
}