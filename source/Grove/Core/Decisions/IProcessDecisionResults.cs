namespace Grove.Decisions
{
  public interface IProcessDecisionResults<in T>
  {
    void ProcessResults(T results);
  }
}