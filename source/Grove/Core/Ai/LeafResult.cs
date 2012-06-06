namespace Grove.Core.Ai
{
  public class LeafResult : ISearchResult
  {
    public LeafResult(int score)
    {
      Score = score;
    }

    public void Visit()
    {
    }

    public int? BestMove
    {
      get { return 0; }
    }

    public int? Score { get; private set; }

    public int? ShortestPath
    {
      get { return 0; }
    }
  }
}