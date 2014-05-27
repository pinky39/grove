namespace Grove.Tests.Performance
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using Grove.Infrastructure;
  using Xunit;

  public class Benchmark
  {
    [Fact]
    public void RunAndCompareWithXUnit()
    {
      var result = RunAndCompare(
        run: BenchmarkRunConfiguration.D40T2MT2(),
        compareTo: BenchmarkRunConfiguration.D40T2MT1());

      Console.WriteLine(result.PrettyPrint());
    }

    [Fact]
    public void RecordWithXUnit()
    {
      var configurations = new[]
        {
          BenchmarkRunConfiguration.D40T2ST(),
          BenchmarkRunConfiguration.D40T2MT1(),
          BenchmarkRunConfiguration.D40T2MT2(),          
        };

      foreach (var configuration in configurations)
      {
        Record(configuration);
      }
    }

    public static void Record(BenchmarkRunConfiguration configuration)
    {      
      var results = Run(configuration);
      results.WriteToFile(configuration.GetBenchmarkBaseFilename());
    }

    public static BenchmarkResult RunAndCompare(BenchmarkRunConfiguration run,
      BenchmarkRunConfiguration compareTo)
    {
      Asrt.True(File.Exists(compareTo.GetBenchmarkBaseFilename()),
        "Benchmark recording file is missing! Please record a benchmark to compare to, before running a comparision.");

      var baseResults = BenchmarkRunResult.ReadFromFile(compareTo.GetBenchmarkBaseFilename());
      var results = Run(run);

      return new BenchmarkResult(results, baseResults);
    }

    private static BenchmarkRunResult Run(BenchmarkRunConfiguration configuration)
    {
      var runs = new List<ScenarioRunResult>();
      var stopwatch = new Stopwatch();

      var tests = typeof (Scenarios)
        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
        .ToArray();

      foreach (var methodInfo in tests)
      {
        var benchmark = new Scenarios(configuration.SearchParameters);

        stopwatch.Start();
        methodInfo.Invoke(benchmark, new object[] {});
        stopwatch.Stop();

        runs.Add(new ScenarioRunResult(
          methodInfo.Name,
          stopwatch.Elapsed));

        stopwatch.Reset();
      }

      return new BenchmarkRunResult(configuration.Name, configuration.Description, runs);
    }
  }
}