namespace Grove.Core.Controllers
{
  public interface IDecision
  {
    void Init();            
    bool HasCompleted { get; }
    bool WasPriorityPassed { get; }
    Player Controller { get; set; }
    Game Game { get; set; }
    void Execute();    
  }  
}