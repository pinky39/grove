namespace Grove.Gameplay.Decisions
{
  public interface IDecision
  {
    bool HasCompleted { get; }
    bool IsPass { get; }    
    void Initialize(Player controller, Game game);
    void Execute();
    void SaveDecisionResults();
  }
}