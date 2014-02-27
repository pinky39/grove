namespace Grove.AI
{
  using System.Text;

  public interface ISearchResult
  {
    int? BestMove { get; }
    int? Score { get; }
    int Color { get; }
    int ChildrenCount { get;  }
    void EvaluateSubtree();

    StringBuilder OutputBestPath(StringBuilder sb = null);
    void CountNodes(TreeSize treeSize);
  }
}