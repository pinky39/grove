namespace Grove.Decisions
{
  using System;

  public class ProcessDecisionResultsHelper<T> : IProcessDecisionResults<T>
  {
    private readonly Action<T> _processResults;

    public ProcessDecisionResultsHelper(Action<T> processResults)
    {
      _processResults = processResults;
    }

    public void ProcessResults(T results)
    {
      _processResults(results);
    }
  }
  
  public interface IProcessDecisionResults<in T>
  {
    void ProcessResults(T results);
  }
}