namespace Grove.Gameplay.Decisions
{
  using Misc;

  public abstract class Decision<TResult> : GameObject, IDecision
  {
    private bool _hasCompleted;
    public TResult Result { get; set; }
    protected virtual bool ShouldExecuteQuery { get { return true; } }
    public Player Controller { get; private set; }

    public virtual bool HasCompleted { get { return _hasCompleted; } }
    public virtual bool IsPass { get { return false; } }

    public virtual void Initialize(Player controller, Game game)
    {
      Controller = controller;
      Game = game;
    }

    public virtual void Execute()
    {
      if (ShouldExecuteQuery)
      {
        ExecuteQuery();
      }
      else
      {
        SetResultNoQuery();
      }

      ProcessResults();
      _hasCompleted = true;
    }

    public virtual void SaveDecisionResults()
    {
      SaveDecisionResult(Result);
    }

    public abstract void ProcessResults();
    protected abstract void ExecuteQuery();
    protected virtual void SetResultNoQuery() {}
  }
}