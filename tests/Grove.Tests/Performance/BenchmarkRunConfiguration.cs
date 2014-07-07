namespace Grove.Tests.Performance
{
  using AI;

  public class BenchmarkRunConfiguration
  {
    public readonly string Description;
    public readonly string Name;
    public readonly SearchParameters SearchParameters;

    public BenchmarkRunConfiguration(string name, string description, SearchParameters searchParameters)
    {
      Description = description;
      Name = name;
      SearchParameters = searchParameters;
    }

    public static BenchmarkRunConfiguration D40T2ST()
    {
      return new BenchmarkRunConfiguration(
        "D40T2ST",
        "Search depth: 40, Targets: 2, Single threaded",
        new SearchParameters(
          searchDepth: 40,
          targetCount: 2,
          searchPartitioningStrategy: SearchPartitioningStrategies.SingleThreaded));
    }

    public static BenchmarkRunConfiguration D40T2MT1()
    {
      return new BenchmarkRunConfiguration(
        "D40T2MT1",
        "Search depth: 40, Targets: 2, Multi threaded (1)",
        new SearchParameters(
          searchDepth: 40,
          targetCount: 2,
          searchPartitioningStrategy: SearchPartitioningStrategies.MultiThreaded1));
    }

    public static BenchmarkRunConfiguration D60T2MT2()
    {
      return new BenchmarkRunConfiguration(
        "D40T2MT2",
        "Search depth: 60, Targets: 2, Multi threaded (2)",
        new SearchParameters(
          searchDepth: 60,
          targetCount: 2,
          searchPartitioningStrategy: SearchPartitioningStrategies.MultiThreaded2));
    }

    public static BenchmarkRunConfiguration D40T2MT2()
    {
      return new BenchmarkRunConfiguration(
        "D40T2MT2",
        "Search depth: 40, Targets: 2, Multi threaded (2)",
        new SearchParameters(
          searchDepth: 40,
          targetCount: 2,
          searchPartitioningStrategy: SearchPartitioningStrategies.MultiThreaded2));
    }

    public static BenchmarkRunConfiguration D30T2ST()
    {
      return new BenchmarkRunConfiguration(
        "D30T2ST",
        "Search depth: 30, Targets: 2, Single threaded",
        new SearchParameters(
          searchDepth: 30,
          targetCount: 2,
          searchPartitioningStrategy: SearchPartitioningStrategies.SingleThreaded));
    }

    public static BenchmarkRunConfiguration D20T1ST()
    {
      return new BenchmarkRunConfiguration(
        "D20T1ST",
        "Search depth: 20, Targets: 1, Single threaded",
        new SearchParameters(
          searchDepth: 20,
          targetCount: 1,
          searchPartitioningStrategy: SearchPartitioningStrategies.SingleThreaded));
    }

    public string GetBenchmarkBaseFilename()
    {
      return string.Format("benchmark-base-{0}.bin", Name);
    }
  }
}