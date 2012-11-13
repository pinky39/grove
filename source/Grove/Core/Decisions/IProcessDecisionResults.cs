namespace Grove.Core.Decisions
{
  public interface IProcessDecisionResults<T>
  {
    void ResultProcessed(T results);
  }
}