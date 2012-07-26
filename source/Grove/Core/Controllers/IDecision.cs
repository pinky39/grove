namespace Grove.Core.Controllers
{
  public interface IDecision
  {
    void Init(Game game, Player controller);
            
    bool HasCompleted { get; }
    bool WasPriorityPassed { get; }
    void Execute();    
  }  
}