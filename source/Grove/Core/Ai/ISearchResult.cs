namespace Grove.Core.Ai
{
  using System.Text;

  public interface ISearchResult
  {
    int? BestMove { get; }
    int? Score { get; }
    void EvaluateSubtree();
    bool IsVisited { get; }

    StringBuilder OutputBestPath(StringBuilder sb = null);
  }
}