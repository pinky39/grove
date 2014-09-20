namespace Grove.AI
{
  public interface IMachineExecutionPlan
  {
    bool ShouldExecuteQuery { get; }
    void ExecuteQuery();
    void ProcessResults();
  }
}