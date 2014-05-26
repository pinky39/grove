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
    private static string BenchmarkBaseFilename
    {
      get { return "benchmarkbase.bin"; }
    }

    [Fact]
    public void CompareToBase()
    {
      var result = RunAndCompare(BenchmarkRunConfiguration.Base());
      Console.WriteLine(result.PrettyPrint());
    }

    [Fact]
    public void Record()
    {
      RecordBase();
    }

    public static void RecordBase(BenchmarkRunConfiguration configuration = null)
    {
      configuration = configuration ?? BenchmarkRunConfiguration.Base();

      var results = Run(configuration);
      results.WriteToFile(BenchmarkBaseFilename);
    }

    public static BenchmarkResult RunAndCompare(BenchmarkRunConfiguration configuration)
    {
      Asrt.True(File.Exists(BenchmarkBaseFilename),
        "Benchmark base file is missing! Please call RecordBase() to record a base benchmark file for this computer.");

      var baseResults = BenchmarkRunResult.ReadFromFile(BenchmarkBaseFilename);
      var results = Run(configuration);

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
        var benchmark = new Scenarios();

        stopwatch.Start();
        methodInfo.Invoke(benchmark, new object[] {});
        stopwatch.Stop();

        runs.Add(new ScenarioRunResult(
          methodInfo.Name,
          stopwatch.Elapsed));

        stopwatch.Reset();
      }

      return new BenchmarkRunResult(configuration.Description, runs);
    }
  }
}