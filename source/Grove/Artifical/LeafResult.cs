namespace Grove.Artifical
{
  using System;
  using System.Text;

  public class LeafResult : ISearchResult
  {
    private readonly int _depth;

    public LeafResult(int score, int depth)
    {
      _depth = depth;
      Score = score;
    }

    public int ChildrenCount { get { return 0; } }

    public void EvaluateSubtree()
    {
      IsVisited = true;
    }

    public bool IsVisited { get; private set; }

    public StringBuilder OutputBestPath(StringBuilder sb)
    {
      sb.Append(Score);
      return sb;
    }

    public void CountNodes(NodeCount count)
    {
      count[_depth]++;
    }

    public int? BestMove { get { return 0; } }
    public int? Score { get; private set; }
  }
}