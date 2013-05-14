namespace Grove.Artifical
{
  using System;

  public class SearchStatistics
  {
    public NodeCount NodeCount;
    public int NumOfWorkersCreated;
    public int SubtreesPrunned;
    public TimeSpan Elapsed;

    public override string ToString()
    {
      return String.Format("Time elapsed: {0}\nNode count: {1}\nNum of workers: {2}\nSubtrees prunned: {3}", Elapsed,
        NodeCount, NumOfWorkersCreated, SubtreesPrunned);
    }
  }
}