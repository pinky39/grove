namespace Grove.Artifical.Decisions
{
  public interface IDecisionExecution
  {
    bool ShouldExecuteQuery { get; }
    void ExecuteQuery();
    void ProcessResults();
  }
}