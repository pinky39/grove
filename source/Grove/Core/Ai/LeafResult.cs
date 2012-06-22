namespace Grove.Core.Ai
{
  using System.Text;

  public class LeafResult : ISearchResult
  {    
    public LeafResult(int score)
    {      
      Score = score;
    }

    public void EvaluateSubtree() {}

    public StringBuilder OutputBestPath(StringBuilder sb)
    {
      sb.Append(Score);
      return sb;
    }

    public int? BestMove { get { return 0; } }

    public int? Score { get; private set; }
  }
}