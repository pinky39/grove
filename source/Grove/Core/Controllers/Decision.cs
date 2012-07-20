namespace Grove.Core.Controllers
{  
  using Infrastructure;
  
  [Copyable]
  public abstract class Decision<TResult> : IDecision where TResult : class
  {
    private bool _hasCompleted;

    public IPlayer Controller { get; private set; }
    public Game Game { get; private set; }

    public TResult Result { get; set; }
    protected virtual bool ShouldExecuteQuery { get { return true; } }
    
    public void Init(Game game, IPlayer controller)
    {
      Game = game;
      Controller = controller;

      Init();
    }
    
    protected virtual void Init(){ }

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