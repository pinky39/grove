namespace Grove.Core.Decisions
{
  using Grove.Infrastructure;

  [Copyable]
  public abstract class Decision<TResult> : IDecision where TResult : class
  {
    private bool _hasCompleted;
    public TResult Result { get; set; }
    protected virtual bool ShouldExecuteQuery { get { return true; } }
    public Player Controller { get; set; }
    public Game Game { get; set; }
    public virtual void Init() {}
    public virtual bool HasCompleted { get { return _hasCompleted; } }
    public virtual bool WasPriorityPassed { get { return false; } }

    public virtual void Execute()
    {
      if (ShouldExecuteQuery)
      {
        ExecuteQuery();
      }

      ProcessResults();
      _hasCompleted = true;
    }

    public abstract void ProcessResults();
    protected abstract void ExecuteQuery();
  }
}