namespace Grove.Decisions
{
  public interface IDecisionHandler
  {
    object Result { get; }
    
    bool HasCompleted { get; }
    bool IsPass { get; }
    void Execute();
    void SaveDecisionResults();

    IDecisionHandler Initialize(object decision, Game game);
  }
}