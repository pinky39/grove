namespace Grove.Decisions
{
  public interface IProcessDecisionResults<T>
  {
    void ProcessResults(T results);
  }
}