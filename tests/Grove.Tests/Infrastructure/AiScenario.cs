namespace Grove.Tests.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Core.Ai;
  using Grove.Infrastructure;

  public abstract class AiScenario : Scenario
  {
    private readonly List<Measurement> _measurements = new List<Measurement>();
    private readonly Stopwatch _stopwatch = new Stopwatch();

    protected AiScenario() : base(
      player1ControlledByScript: false,
      player2ControlledByScript: false)
    {
      Search.Started += delegate
        {
          _stopwatch.Start();
        };

      Search.Finished += delegate
        {
          _stopwatch.Stop();          
          _measurements.Add(new Measurement
            {
              NodeCount = Search.NodeCount,
              WorkerCount = Search.NumWorkersCreated,
              SubtreesPrunned = Search.SubtreesPrunned,
              ElapsedTime = _stopwatch.ElapsedMilliseconds
            });

          _stopwatch.Reset();
        };
    }

    protected int AvarageNodeCount
    {
      get { return SearchCount > 0 ? TotalNodesCount/SearchCount : 0; }
    }

    protected double AvarageSpeed
    {
      get { return TotalNodesCount > 0 ? TotalNodesCount/TotalElapsedTime : 0; }
    }

    protected int AvarageWorkerCount
    {
      get { return SearchCount > 0 ? TotalWorkerCount/SearchCount : 0; }
    }

    protected NodeCount MaxNodeCount
    {
      get
      {

        var max = new NodeCount();
        
        
        foreach (var measurement in _measurements)
        {
          if (measurement.NodeCount.Total > max.Total)
          {
            max = measurement.NodeCount;
          }
        }

        return max;
      }
    }

    protected double MaxSearchTime
    {
      get { return _measurements.Any() ? _measurements.Max(x => x.ElapsedTime)/1000.0d : 0; }
    }

    protected int MaxWorkerCount
    {
      get { return _measurements.Any() ? _measurements.Max(x => x.WorkerCount) : 0; }
    }

    private int SearchCount
    {
      get { return _measurements.Count; }
    }

    protected double TotalElapsedTime
    {
      get { return _measurements.Sum(x => x.ElapsedTime)/1000.0d; }
    }

    protected int TotalNodesCount
    {
      get { return _measurements.Sum(x => x.NodeCount.Total); }
    }    

    protected int MaxSubtreesPrunned
    {
      get { return _measurements.Any() ? _measurements.Max(x => x.SubtreesPrunned) : 0; }
    }

    protected int TotalSubtreesPrunned
    {
      get { return _measurements.Sum(x => x.SubtreesPrunned); }
    }

    protected int TotalWorkerCount
    {
      get { return _measurements.Sum(x => x.WorkerCount); }
    }

    protected override void RunGame(int maxTurnCount)
    {
      InitializeCopyCache();      
      base.RunGame(maxTurnCount);
    }

    private void InitializeCopyCache()
    {
      // this is done to get more accurate performance measurements
      var copyService = new CopyService();
      copyService.CopyRoot(Game);
    }

    public override void Dispose()
    {
      _stopwatch.Stop();

      Console.WriteLine(@"Search count: {0}", SearchCount);
      Console.WriteLine(@"Total search time: {0:f2} seconds", TotalElapsedTime);
      Console.WriteLine(@"Total node count: {0}", TotalNodesCount);
      Console.WriteLine(@"Total subtrees prunned: {0}", TotalSubtreesPrunned);
      Console.WriteLine(@"Max subtrees prunned: {0}", MaxSubtreesPrunned);
      Console.WriteLine(@"Total worker count: {0}", TotalWorkerCount);
      Console.WriteLine(@"Max node count: {0}", MaxNodeCount);
      Console.WriteLine(@"Max worker count: {0}", MaxWorkerCount);
      Console.WriteLine(@"Max search time: {0:f2} seconds", MaxSearchTime);
      Console.WriteLine(@"Avarage node count: {0}", AvarageNodeCount);
      Console.WriteLine(@"Avarage worker count: {0}", AvarageWorkerCount);
      Console.WriteLine(@"Avarage speed: {0:f2} nodes/second", AvarageSpeed);
    }
    
    private class Measurement
    {
      public long ElapsedTime { get; set; }
      public NodeCount NodeCount { get; set; }
      public int WorkerCount { get; set; }
      public int SubtreesPrunned { get; set; }
    }    
  }
}