namespace Grove.Core.Controllers.Machine
{
  public interface IDecisionExecution
  {
    bool ShouldExecuteQuery { get; }
    void ExecuteQuery();
    void ProcessResults();
  }
}