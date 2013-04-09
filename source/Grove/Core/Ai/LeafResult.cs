namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  public class LeafResult : ISearchResult
  {
    private readonly int _stepCount;

    public LeafResult(int score, int stepCount)
    {
      _stepCount = stepCount;
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

    public void CountNodes(NodeCount count)
    {
      count[_stepCount]++;
    }

    public int? BestMove { get { return 0; } }
    public int? Score { get; private set; }
  }
}