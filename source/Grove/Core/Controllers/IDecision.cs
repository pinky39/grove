namespace Grove.Core.Controllers
{
  public interface IDecision
  {
    bool HasCompleted { get; }
    bool WasPriorityPassed { get; }
    void Execute();
  }
}