namespace Grove.AI
{
  public interface IMachineExecutionPlan
  {
    bool ShouldExecuteQuery { get; }
    void SetResultNoQuery();
    void ExecuteQuery();
    void SaveAndProcessResults();
  }
}