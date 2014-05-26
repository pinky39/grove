namespace Grove.Tests.Performance
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.Serialization.Formatters;
  using System.Runtime.Serialization.Formatters.Binary;
  using System.Text;

  [Serializable]
  public class ScenarioRunResult
  {
    public readonly TimeSpan Duration;
    public readonly string Name;

    public ScenarioRunResult(string name, TimeSpan duration)
    {
      Name = name;
      Duration = duration;
    }
  }

  public class BenchmarkResult
  {
    private readonly List<BenchmarkRunResultComparision> _comparisions = new List<BenchmarkRunResultComparision>();
    private readonly string _description;

    public BenchmarkResult(BenchmarkRunResult newResult, BenchmarkRunResult baseResult)
    {
      _description = newResult.Description;

      var baseDictionary = baseResult.Runs.ToDictionary(x => x.Name, x => x);

      foreach (var run in newResult.Runs)
      {
        var baseDuration = baseDictionary.ContainsKey(run.Name)
          ? baseDictionary[run.Name].Duration
          : run.Duration;


        _comparisions.Add(new BenchmarkRunResultComparision(run.Name, run.Duration, baseDuration));
      }
    }

    private TimeSpan Duration { get { return _comparisions.Aggregate(new TimeSpan(), (total, x) => total + x.Duration); } }
    private TimeSpan BaseDuration { get { return _comparisions.Aggregate(new TimeSpan(), (total, x) => total + x.BaseDuration); } }
    private double Improvement { get { return (BaseDuration - Duration).TotalMilliseconds/(BaseDuration.TotalMilliseconds); } }

    public string PrettyPrint()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("Benchmark '{0}'\n\n", _description);
      sb.AppendFormat("Duration: {0:0.00} s\n", Duration.TotalSeconds);
      sb.AppendFormat("Base duration: {0:0.00} s\n", BaseDuration.TotalSeconds);
      sb.AppendFormat("Improvement: {0:0.00} %\n\n", Improvement*100);


      const string lineformat = "{0,15}{1,20}{2,20}{3,20}\n";
      sb.AppendFormat(lineformat, "Name", "Duration (ms)", "Base duration (ms)", "Improovement (%)");
      foreach (var comparision in _comparisions)
      {
        sb.AppendFormat(lineformat,
          comparision.Name,
          comparision.Duration.TotalMilliseconds.ToString("f0"),
          comparision.BaseDuration.TotalMilliseconds.ToString("f0"),
          (comparision.Improovement*100).ToString("0.00"));
      }

      return sb.ToString();
    }

    private class BenchmarkRunResultComparision
    {
      public readonly TimeSpan BaseDuration;
      public readonly TimeSpan Duration;
      public readonly string Name;

      public BenchmarkRunResultComparision(string name, TimeSpan duration, TimeSpan baseDuration)
      {
        Name = name;
        Duration = duration;
        BaseDuration = baseDuration;
      }

      public double Improovement { get { return (BaseDuration - Duration).TotalMilliseconds/(BaseDuration.TotalMilliseconds); } }
    }
  }

  [Serializable]
  public class BenchmarkRunResult
  {
    public readonly string Description;
    public readonly List<ScenarioRunResult> Runs;

    public BenchmarkRunResult(string description, List<ScenarioRunResult> runs)
    {
      Description = description;
      Runs = runs;
    }

    public void WriteToFile(string filename)
    {
      var formatter = CreateFormatter();

      using (var stream = new FileStream(filename, FileMode.Create))
      {
        formatter.Serialize(stream, this);
      }
    }

    public static BenchmarkRunResult ReadFromFile(string filename)
    {
      var formatter = CreateFormatter();

      using (var stream = new FileStream(filename, FileMode.Open))
      {
        return (BenchmarkRunResult) formatter.Deserialize(stream);
      }
    }

    private static BinaryFormatter CreateFormatter()
    {
      var formatter = new BinaryFormatter
        {
          AssemblyFormat = FormatterAssemblyStyle.Simple
        };
      return formatter;
    }
  }
}