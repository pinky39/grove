namespace Grove.Artifical
{
  using System;

  public class SearchStatistics
  {
    public TreeSize SearchTreeSize;
    public int NumOfWorkersCreated;
    public int SubtreesPrunned;
    public TimeSpan Elapsed;
    public int NodeCount;

    public override string ToString()
    {
      return String.Format("Time elapsed: {0}\nNode count: {1}\nTree size: {2}\nNum of workers: {3}\nSubtrees prunned: {4}", Elapsed, NodeCount,
        SearchTreeSize, NumOfWorkersCreated, SubtreesPrunned);
    }
  }
}