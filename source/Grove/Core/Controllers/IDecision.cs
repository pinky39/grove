namespace Grove.Core.Controllers
{
  public interface IDecision
  {
    void Init(Game game, IPlayer controller);
            
    bool HasCompleted { get; }
    bool WasPriorityPassed { get; }
    void Execute();    
  }  
}