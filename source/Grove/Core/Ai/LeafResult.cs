namespace Grove.Core.Ai
{
  using System.Text;

  public class LeafResult : ISearchResult
  {
    public LeafResult(int score)
    {
      Score = score;
    }

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

    public int? BestMove { get { return 0; } }
    public int? Score { get; private set; }
  }
}