namespace Grove.Core.Controllers.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;
  using Results;

  public class SetTriggeredAbilityTarget : Controllers.SetTriggeredAbilityTarget, ISearchNode, IDecisionExecution
  {
    private readonly DecisionExecutor _executor;
    private List<ITarget> _targets;

    private SetTriggeredAbilityTarget() {}

    public SetTriggeredAbilityTarget(ChangeTracker changeTracker)
    {
      _executor = new DecisionExecutor(this, changeTracker);
      Result = DefaultResult();
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }
    public bool IsMax { get { return Player.IsMax; } }
    public Search Search { get; set; }

    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }


    public Game Game { get; set; }
    public int ResultCount { get { return _targets.Count; } }

    public void GenerateChoices()
    {
      _targets = GenerateTargets().ToList();
    }

    public void SetResult(int index)
    {
      Result = new ChosenTarget(_targets[index]);
    }

    public override void Execute()
    {
      _executor.Execute();
    }

    protected override void ExecuteQuery()
    {
      Search.SetBestResult(this);
    }

    private static ChosenTarget DefaultResult()
    {
      return new ChosenTarget(null);
    }

    private IEnumerable<ITarget> GenerateTargets()
    {
      var targets = new TargetGenerator(
        TargetSelector,
        Game.Players,
        Game.Stack,
        maxX: null,
        maxTargets: Search.TargetLimit
        );

      if (targets.None())
      {
        yield return null;
        yield break;
      }

      foreach (var target in targets)
      {
        yield return target;
      }
    }
  }
}