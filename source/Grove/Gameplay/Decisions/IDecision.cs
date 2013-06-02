namespace Grove.Gameplay.Decisions
{
  public interface IDecision
  {
    bool HasCompleted { get; }
    bool WasPriorityPassed { get; }
    void Initialize(Player controller, Game game);
    void Execute();
    void SaveDecisionResults();
  }
}