namespace Grove.Core.Decisions.Machine
{
  public interface IDecisionExecution
  {
    bool ShouldExecuteQuery { get; }
    void ExecuteQuery();
    void ProcessResults();
  }
}