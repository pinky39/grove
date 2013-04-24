namespace Grove.Gameplay.Decisions
{
  public interface IProcessDecisionResults<T>
  {
    void ProcessResults(T results);
  }
}