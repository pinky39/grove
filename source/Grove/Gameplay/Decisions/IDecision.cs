namespace Grove.Gameplay.Decisions
{
  using Player;

  public interface IDecision
  {
    bool HasCompleted { get; }
    bool WasPriorityPassed { get; }
    void Initialize(Player controller, Game game);
    void Execute();
  }
}