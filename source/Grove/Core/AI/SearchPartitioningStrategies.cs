namespace Grove.AI
{
  public delegate bool SearchPartitioningStrategy(SearchPartitioningStrategyParameters p);

  public static class SearchPartitioningStrategies
  {
    public static SearchPartitioningStrategy DefaultMultithreaded = MultiThreaded2;
    
    public static bool SingleThreaded(SearchPartitioningStrategyParameters p)
    {
      return false;
    }

    public static bool MultiThreaded1(SearchPartitioningStrategyParameters p)
    {
      const int maxWorkers = 4;
      return (p.Search.WorkerCount < maxWorkers && p.SearchDepth < 10 && p.Node.ResultCount > 3 &&
        (p.SearchDepth%4 == 0));
    }

    public static bool MultiThreaded2(SearchPartitioningStrategyParameters p)
    {
      const int maxWorkers = 6;
      return (p.Search.WorkerCount < maxWorkers && p.SearchDepth < 2 && p.MoveIndex < p.Node.ResultCount - 1);
    }
  }
}