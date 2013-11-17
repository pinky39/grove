namespace Grove.Infrastructure
{
  using System;
  using System.Diagnostics;

  public static class Profiler
  {
    public static double Benchmark(Action action)
    {
      var stopWatch = new Stopwatch();

      stopWatch.Start();
      action();
      stopWatch.Stop();

      return stopWatch.Elapsed.TotalMilliseconds;
    }
  }
}