namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using log4net;
  using Messages;

  public class Search
  {
    public const int TargetLimit = 2;
    private static readonly ILog Log = LogManager.GetLogger(typeof (Search));
    private readonly SearchResults _searchResults;
    private readonly Dictionary<object, SearchWorker> _workers = new Dictionary<object, SearchWorker>();
    private readonly object _workersLock = new object();
    private int _nodesSearched;
    private int _numWorkersCreated;
    private int _startStateCount;
    private int _startStepCount;
    private int _subtreesPrunned;

    public Search(SearchResults searchResults)
    {
      _searchResults = searchResults;
      SearchDepth = 16;
    }

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

    public int MaxDepth { get { return _startStepCount + SearchDepth; } }
    public int NodesSearched { get { return _nodesSearched; } }
    public int NumWorkersCreated { get { return _numWorkersCreated; } }
    public int SearchDepth { get; set; }
    public int SubtreesPrunned { get { return _subtreesPrunned; } }
    public event EventHandler Finished = delegate { };
    public event EventHandler Started = delegate { };

    public Task ExecuteTask(SearchWorker parentWorker, ISearchNode parentNode, int resultIndex,
                            Action<SearchWorker, ISearchNode> action)
    {
      var worker = parentWorker;
      var node = parentNode;

      lock (_workersLock)
      {
        if (IsItFeasibleToCreateNewWorker(parentNode, resultIndex))
        {
          worker = CreateWorker(node);
          node = worker.Root;
        }
      }

      var task = new Task(() => action(worker, node));

      if (worker != parentWorker)
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

      searchNode.Game.Players.SetAiVisibility(searchNode.Player);
      searchNode.GenerateChoices();

      var result =
        GetCachedResult(searchNode) ??
          FindBestMove(searchNode);

      searchNode.SetResult(result);
    }

    private SearchWorker CreateWorker(ISearchNode searchNode)
    {
      var worker = new SearchWorker(this, _searchResults, searchNode);

      lock (_workersLock)
      {
        _workers.Add(worker.Id, worker);
        _numWorkersCreated++;
      }

      return worker;
    }

    private int FindBestMove(ISearchNode searchNode)
    {
      searchNode.Game.ChangeTracker.Enable();
      searchNode.Game.ChangeTracker.Lock();

      _startStepCount = searchNode.Game.Turn.StepCount;
      _startStateCount = searchNode.Game.Turn.StateCount;

      Log.Debug("Search started");
      searchNode.Game.Publisher.Publish(new SearchStarted());

      _nodesSearched = 0;
      _subtreesPrunned = 0;
      _numWorkersCreated = 0;
      _searchResults.Clear();

      Started(this, EventArgs.Empty);

      var worker = CreateWorker(searchNode);
      worker.Evaluate();
      RemoveWorker(worker);

      Finished(this, EventArgs.Empty);
      searchNode.Game.Publisher.Publish(new SearchFinished());
      Log.Debug("Search finished");

      searchNode.Game.ChangeTracker.Disable();
      searchNode.Game.ChangeTracker.Unlock();

      var root = GetSearchNodeResult(searchNode);
      root.EvaluateSubtree();

      Log.DebugFormat("Best path: {0}", root.OutputBestPath());

      GC.Collect();

      return root.BestMove.Value;
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
      return SingleThreadedStrategy(node, moveIndex);
      //return MultiThreadedStrategy2(node, moveIndex);
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

      Interlocked.Add(ref _nodesSearched, worker.NodesSearched);
      Interlocked.Add(ref _subtreesPrunned, worker.SubTreesPrunned);
    }
  }
}