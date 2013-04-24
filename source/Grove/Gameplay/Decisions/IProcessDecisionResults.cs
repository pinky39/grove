namespace Grove.Core.Decisions
{
  public interface IProcessDecisionResults<T>
  {
    void ProcessResults(T results);
  }
}