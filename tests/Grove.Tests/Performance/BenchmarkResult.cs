namespace Grove.Tests.Performance
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class BenchmarkResult
  {
    private readonly BenchmarkRunResult _baseResult;
    private readonly List<BenchmarkRunResultComparision> _comparisions = new List<BenchmarkRunResultComparision>();
    private readonly BenchmarkRunResult _newResult;

    public BenchmarkResult(BenchmarkRunResult newResult, BenchmarkRunResult baseResult)
    {
      _newResult = newResult;
      _baseResult = baseResult;

      var baseDictionary = baseResult.Runs.ToDictionary(x => x.Name, x => x);

      foreach (var run in newResult.Runs)
      {
        var baseDuration = baseDictionary.ContainsKey(run.Name)
          ? baseDictionary[run.Name].Duration
          : run.Duration;


        _comparisions.Add(new BenchmarkRunResultComparision(run.Name, run.Duration, baseDuration));
      }
    }

    private TimeSpan Duration { get { return _newResult.Duration; } }
    private TimeSpan BaseDuration { get { return _baseResult.Duration; } }
    private double Improvement { get { return (BaseDuration - Duration).TotalMilliseconds/(BaseDuration.TotalMilliseconds); } }

    public string PrettyPrint()
    {
      var sb = new StringBuilder();
      sb.AppendFormat("Benchmark '{0}'\n\n", _newResult.Description);      
      sb.AppendFormat("Duration: {0:0.00} s\n", Duration.TotalSeconds);
      sb.AppendFormat("Base: '{0}' recorded on {1}.\n", _baseResult.Name, _baseResult.RecordedAt);
      sb.AppendFormat("Base duration: {0:0.00} s\n", BaseDuration.TotalSeconds);      
      sb.AppendFormat("Improvement: {0:0.00} %\n\n", Improvement*100);


      const string lineformat = "{0,15}{1,20}{2,20}{3,20}\n";
      sb.AppendFormat(lineformat, "Name", "Duration (ms)", "Base duration (ms)", "Improovement (%)");
      foreach (var comparision in _comparisions.OrderBy(x => x.Name))
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
}