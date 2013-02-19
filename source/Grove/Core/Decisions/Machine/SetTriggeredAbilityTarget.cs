namespace Grove.Core.Decisions.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;
  using Results;
  using Targeting;

  public class SetTriggeredAbilityTarget : Decisions.SetTriggeredAbilityTarget, ISearchNode, IDecisionExecution
  {
    private readonly DecisionExecutor _executor;
    private List<Targets> _targets;

    public SetTriggeredAbilityTarget()
    {
      Result = DefaultResult();
      _executor = new DecisionExecutor(this);
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }
    public bool IsMax { get { return Controller.IsMax; } }

    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    Game ISearchNode.Game { get { return Game; } }

    public int ResultCount { get { return _targets.Count; } }

    public void GenerateChoices()
    {
      _targets = GenerateTargets().ToList();
    }

    public void SetResult(int index)
    {
      Result = new ChosenTargets(_targets[index]);
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
      Search.SetBestResult(this);
    }

    private static ChosenTargets DefaultResult()
    {
      return new ChosenTargets(null);
    }

    private IEnumerable<Targets> GenerateTargets()
    {
      var generator = new TargetGenerator(
        TargetSelector,
        Source.OwningCard,
        Game,
        maxX: null,
        forceOne: true
        );

      if (generator.None())
      {
        yield return null;
        yield break;
      }

      foreach (var targets in generator.Take(Search.MaxTargetCandidates))
      {
        yield return targets;
      }
    }

    public override string ToString()
    {
      return string.Format("{0}: {1} sets trig. ability targets", Game.Turn.Step, Controller);
    }
  }
}