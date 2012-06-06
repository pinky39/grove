namespace Grove.Core.Ai
{
  public interface ISearchResult
  {
    void Visit();
    
    int? BestMove { get; }
    int? Score { get; }
    int? ShortestPath { get; }
  }
}