namespace Grove.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Core;
  using Gameplay;
  using Gameplay.Messages;
  using Grove.Infrastructure;
  using log4net;

  public class Search
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (Search));
    private readonly Queue<int> _lastDurations = new Queue<int>(new[] {0});
    private readonly int _maxSearchDepth;
    private readonly int _maxTargetCount;
    private readonly SearchResults _searchResults = new SearchResults();
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly Dictionary<object, SearchWorker> _workers = new Dictionary<object, SearchWorker>();
    private readonly object _workersLock = new object();
    private ISearchResult _lastSearchResult;
    private int _numWorkersCreated;
    private int _startStateCount;
    private int _startStepCount;
    private int _subtreesPrunned;

    public Search(int maxSearchDepth, int maxTargetCount)
    {
      _maxSearchDepth = maxSearchDepth;
      _maxTargetCount = maxTargetCount;

      SearchDepthLimit = _maxSearchDepth;
      MaxTargetCandidates = _maxTargetCount;
    }

    public int MaxTargetCandidates { get; private set; }

    public bool InProgress
    {
      get
      {
        lock (_workersLock)
        {
          return _workers.Count > 0;
        }
      }
    }

    public int MaxDepth { get { return _startStepCount + SearchDepthLimit; } }
    public int PlaySpellUntilDepth { get; private set; }
    public int NumWorkersCreated { get { return _numWorkersCreated; } }
    public int SearchDepthLimit { get; private set; }
    public int SubtreesPrunned { get { return _subtreesPrunned; } }

    public NodeCount NodeCount
    {
      get
      {
        var count = new NodeCount();

        if (_lastSearchResult != null)
          _lastSearchResult.CountNodes(count);

        return count;
      }
    }

    public event EventHandler Finished = delegate { };
    public event EventHandler Started = delegate { };

    public int GetSearchDepthInSteps(int currentStepCount)
    {
      return currentStepCount - _startStepCount;
    }


    public SearchWorker GetWorker(object id)
    {
      lock (_workersLock)
      {
        return _workers[id];
      }
    }

    public void SetBestResult(ISearchNode searchNode)
    {
      if (InProgress)
      {
        var worker = GetWorker(searchNode.Game);

        searchNode.GenerateChoices();
        worker.Evaluate(searchNode);
        return;
      }

      searchNode.Game.Players.Searching = searchNode.Controller;
      searchNode.GenerateChoices();

      var result =
        GetCachedResult(searchNode) ??
          FindBestMove(searchNode);

      searchNode.SetResult(result);
    }

    private SearchWorker CreateWorker(InnerResult rootResult, Game game)
    {
      var worker = new SearchWorker(rootResult, game, _searchResults);

      lock (_workersLock)
      {
        _workers.Add(worker.Id, worker);
        _numWorkersCreated++;
      }

      return worker;
    }

    private int FindBestMove(ISearchNode searchNode)
    {
      InitSearch(searchNode);

      var rootNode = new CopyService().CopyRoot(searchNode);

      var rootResult = new InnerResult(
        rootNode.Game.CalculateHash(), 
        rootNode.Controller.IsMax, 0);
      
      var worker = CreateWorker(rootResult, rootNode.Game);

      worker.StartSearch(rootNode);
      
      FinishSearch(searchNode, worker);

      return GetBestMove(searchNode);
    }

    private int GetBestMove(ISearchNode searchNode)
    {
      _lastSearchResult = GetSearchNodeResult(searchNode);
      _lastSearchResult.EvaluateSubtree();
      return _lastSearchResult.BestMove.Value;
    }

    private void FinishSearch(ISearchNode searchNode, SearchWorker worker)
    {
      RemoveWorker(worker);

      _stopwatch.Stop();

      if (_lastDurations.Count == 10)
      {
        _lastDurations.Dequeue();
      }

      _lastDurations.Enqueue(((int) _stopwatch.Elapsed.TotalMilliseconds));

      Finished(this, EventArgs.Empty);
      searchNode.Game.Publish(new SearchFinished());
      Log.Debug("Search finished");

      searchNode.Game.ChangeTracker.Disable();
      searchNode.Game.ChangeTracker.Unlock();

      GC.Collect();
    }

    private void InitSearch(ISearchNode searchNode)
    {
      AdjustPerformance();

      Log.Debug("Search started");

      searchNode.Game.Publish(new SearchStarted
        {
          SearchDepthLimit = SearchDepthLimit,
          TargetCountLimit = MaxTargetCandidates
        });

      Started(this, EventArgs.Empty);

      _stopwatch.Reset();
      _stopwatch.Start();

      _subtreesPrunned = 0;
      _numWorkersCreated = 0;
      _searchResults.Clear();

      searchNode.Game.ChangeTracker.Enable();
      searchNode.Game.ChangeTracker.Lock();
      _startStepCount = searchNode.Game.Turn.StepCount;
      _startStateCount = searchNode.Game.Turn.StateCount;
      PlaySpellUntilDepth = searchNode.Game.Turn.GetStepCountAtNextTurnCleanup();      
    }

    private void AdjustPerformance()
    {
      if (_lastDurations.Last() > 4000)
      {
        if (MaxTargetCandidates > 1)
        {
          MaxTargetCandidates--;
        }
        if (SearchDepthLimit > 1)
        {
          SearchDepthLimit -= Math.Min(4, SearchDepthLimit - 1);
        }
      }

      if (_lastDurations.None(x => x > 1500))
      {
        if (SearchDepthLimit < _maxSearchDepth)
        {
          SearchDepthLimit += 2;
        }
        else if (MaxTargetCandidates < _maxTargetCount)
        {
          MaxTargetCandidates++;
        }
      }
    }

    private int? GetCachedResult(ISearchNode searchNode)
    {
      if (searchNode.ResultCount == 1)
        return 0;

      var result = GetSearchNodeResult(searchNode);
      return result == null ? null : result.BestMove;
    }

    private ISearchResult GetSearchNodeResult(ISearchNode searchNode)
    {
      return _searchResults.GetResult(searchNode.Game.CalculateHash());
    }

    public int GetDepth(int stateCount)
    {
      return stateCount - _startStateCount;
    }

    private bool IsItFeasibleToCreateNewWorker(ISearchNode node, int moveIndex)
    {
#if DEBUG
      //return SingleThreadedStrategy(node, moveIndex);
      return MultiThreadedStrategy2(node, moveIndex);
#else
      return MultiThreadedStrategy2(node, moveIndex);
#endif
    }

    private static bool SingleThreadedStrategy(ISearchNode node, int moveIndex)
    {
      return false;
    }

    private bool MultiThreadedStrategy1(ISearchNode node, int moveIndex)
    {
      const int maxWorkers = 4;
      var depth = GetDepth(node.Game.Turn.StateCount);
      return (_workers.Count < maxWorkers && depth == 0 && moveIndex > 0);
    }

    private bool MultiThreadedStrategy2(ISearchNode node, int moveIndex)
    {
      const int maxWorkers = 6;
      var depth = GetDepth(node.Game.Turn.StateCount);
      return (_workers.Count < maxWorkers && depth < 2 && moveIndex < node.ResultCount - 1);
    }

    private void RemoveWorker(SearchWorker worker)
    {
      lock (_workersLock)
      {
        var key = _workers.Single(x => x.Value == worker).Key;
        _workers.Remove(key);
      }

      Interlocked.Add(ref _subtreesPrunned, worker.SubTreesPrunned);
    }

    public Task EvaluateBranch(SearchWorker worker, ISearchNode rootNode, InnerResult rootResult, int moveIndex)
    {
      var shouldCreateNewWorker = false;
      lock (_workersLock)
      {
        shouldCreateNewWorker = IsItFeasibleToCreateNewWorker(rootNode, moveIndex);

        if (shouldCreateNewWorker)
        {
          rootNode = new CopyService().CopyRoot(rootNode);
          worker = CreateWorker(rootResult, rootNode.Game);
        }
      }

      var task = new Task(() => worker.EvaluateBranch(moveIndex, rootNode, rootResult));


      if (shouldCreateNewWorker)
      {
        var continueWith = task.ContinueWith(
          t => RemoveWorker(worker),
          TaskContinuationOptions.ExecuteSynchronously);

        task.Start();

        return continueWith;
      }

      task.RunSynchronously();
      return task;
    }
  }
}