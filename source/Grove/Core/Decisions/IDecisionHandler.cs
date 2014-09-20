namespace Grove.Decisions
{
  public interface IDecisionHandler
  {
    bool HasCompleted { get; }
    bool IsPass { get; }
    void Execute();
    void SaveDecisionResults();

    IDecisionHandler Initialize(object decision, Game game);
  }
}