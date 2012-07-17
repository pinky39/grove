namespace Grove.Core.Controllers.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;
  using Results;
  using Targeting;

  public class SetTriggeredAbilityTarget : Controllers.SetTriggeredAbilityTarget, ISearchNode, IDecisionExecution
  {
    private DecisionExecutor _executor;
    private List<Targets> _targets;    

    public SetTriggeredAbilityTarget()
    {
      
      Result = DefaultResult();
    }

    protected override void Init()
    {
      _executor = new DecisionExecutor(this, Game.ChangeTracker);
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }
    public bool IsMax { get { return Controller.IsMax; } }
    public Search Search { get { return Game.Search; } }

    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }
    
    public int ResultCount { get { return _targets.Count; } }

    public void GenerateChoices()
    {
      _targets = GenerateTargets().ToList();
    }

    public void SetResult(int index)
    {
      Result = new ChosenTargets(_targets[index]);
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
        TargetSelectors,
        Effect.Source.OwningCard,
        Game,
        maxX: null,
        forceOne: true
        );

      if (generator.None())
      {
        yield return null;
        yield break;
      }

      foreach (var targets in generator.Take(Search.TargetLimit))
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