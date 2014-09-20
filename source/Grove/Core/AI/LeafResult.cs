namespace Grove.AI
{
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
      Color = 1;
    }

    public int Color { get; private set; }

    public StringBuilder OutputBestPath(StringBuilder sb)
    {
      sb.Append(Score);
      return sb;
    }

    public void CountNodes(TreeSize treeSize)
    {      
      if (Color == 2)
        return;
      
      Color = 2;
      treeSize[_depth]++;      
    }

    public int? BestMove { get { return 0; } }
    public int? Score { get; private set; }
  }
}