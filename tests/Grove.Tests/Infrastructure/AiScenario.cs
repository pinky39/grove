namespace Grove.Tests.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.AI;
  using Grove.Infrastructure;

  public abstract class AiScenario : Scenario
  {
    private readonly List<Measurement> _measurements = new List<Measurement>();

    protected AiScenario() : base(
      player1ControlledByScript: false,
      player2ControlledByScript: false)
    {
      Game.Ai.SearchFinished += delegate
        {
          _measurements.Add(new Measurement
            {
              TreeSize = Game.Ai.LastSearchStatistics.SearchTreeSize,
              NodeCount = Game.Ai.LastSearchStatistics.NodeCount,
              WorkerCount = Game.Ai.LastSearchStatistics.NumOfWorkersCreated,
              SubtreesPrunned = Game.Ai.LastSearchStatistics.SubtreesPrunned,
              ElapsedTime = Game.Ai.LastSearchStatistics.Elapsed.TotalMilliseconds
            });
        };
    }

    protected int AvarageTreeSize { get { return SearchCount > 0 ? TotalTreeSize/SearchCount : 0; } }

    protected double AvarageSpeed { get { return TotalNodeCount > 0 ? TotalNodeCount/TotalElapsedTime : 0; } }

    protected int AvarageWorkerCount { get { return SearchCount > 0 ? TotalWorkerCount/SearchCount : 0; } }

    protected TreeSize MaxTreeSize
    {
      get
      {
        var max = new TreeSize();


        foreach (var measurement in _measurements)
        {
          if (measurement.TreeSize.Total > max.Total)
          {
            max = measurement.TreeSize;
          }
        }

        return max;
      }
    }

    protected int MaxNodeCount
    {
      get { return _measurements.Any() ? _measurements.Max(x => x.NodeCount) : 0; }
    }

    protected int TotalNodeCount
    {
      get { return _measurements.Sum(x => x.NodeCount); }
    }

    protected double AvarageNodeCount
    {
      get { return _measurements.Any() ? _measurements.Average(x => x.NodeCount) : 0; }
    }

    protected double MaxSearchTime { get { return _measurements.Any() ? _measurements.Max(x => x.ElapsedTime)/1000.0d : 0; } }

    protected int MaxWorkerCount { get { return _measurements.Any() ? _measurements.Max(x => x.WorkerCount) : 0; } }

    private int SearchCount { get { return _measurements.Count; } }

    protected double TotalElapsedTime { get { return _measurements.Sum(x => x.ElapsedTime)/1000.0d; } }

    protected int TotalTreeSize { get { return _measurements.Sum(x => x.TreeSize.Total); } }

    protected int MaxSubtreesPrunned { get { return _measurements.Any() ? _measurements.Max(x => x.SubtreesPrunned) : 0; } }

    protected int TotalSubtreesPrunned { get { return _measurements.Sum(x => x.SubtreesPrunned); } }

    protected int TotalWorkerCount { get { return _measurements.Sum(x => x.WorkerCount); } }

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
      Console.WriteLine(@"Search count: {0}", SearchCount);
      Console.WriteLine(@"Total search time: {0:f2} seconds", TotalElapsedTime);
      Console.WriteLine(@"Total tree size: {0}", TotalTreeSize);
      Console.WriteLine(@"Total node count: {0}", TotalNodeCount);
      Console.WriteLine(@"Total subtrees prunned: {0}", TotalSubtreesPrunned);
      Console.WriteLine(@"Max subtrees prunned: {0}", MaxSubtreesPrunned);
      Console.WriteLine(@"Total worker count: {0}", TotalWorkerCount);
      Console.WriteLine(@"Max tree size: {0}", MaxTreeSize);
      Console.WriteLine(@"Max node count: {0}", MaxNodeCount);
      Console.WriteLine(@"Max worker count: {0}", MaxWorkerCount);
      Console.WriteLine(@"Max search time: {0:f2} seconds", MaxSearchTime);
      Console.WriteLine(@"Avarage tree size: {0}", AvarageTreeSize);
      Console.WriteLine(@"Avarage node count: {0}", AvarageNodeCount);
      Console.WriteLine(@"Avarage worker count: {0}", AvarageWorkerCount);
      Console.WriteLine(@"Avarage speed: {0:f2} nodes/second", AvarageSpeed);
    }

    private class Measurement
    {
      public int NodeCount { get; set; }
      public double ElapsedTime { get; set; }
      public TreeSize TreeSize { get; set; }
      public int WorkerCount { get; set; }
      public int SubtreesPrunned { get; set; }
    }
  }
}