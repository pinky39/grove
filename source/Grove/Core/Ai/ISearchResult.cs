namespace Grove.Core.Ai
{
  public interface ISearchResult
  {
    int BestMove { get; }
    int Score { get; }
    int ShortestPath { get; }
  }
}