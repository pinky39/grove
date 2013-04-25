namespace Grove.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Gameplay;
  using Gameplay.Player;
  using Infrastructure;

  public class Search
  {
    private readonly Game _game;
    private readonly SearchParameters _p;
    private readonly InnerResult _root;
    private readonly SearchResults _searchResults = new SearchResults();
    private readonly Dictionary<object, SearchWorker> _workers = new Dictionary<object, SearchWorker>();
    private readonly object _workersLock = new object();
    private int _numWorkersCreated;
    private int _subtreesPrunned;

    public Search(SearchParameters p, Player searchingPlayer, Game game)
    {
      _p = p;
      _game = game;      
      _root = new InnerResult(_game.CalculateHash(), searchingPlayer.IsMax, 0);
    }

    public int TargetCount { get { return _p.TargetCount; } }

    public int Result { get { return _root.BestMove.GetValueOrDefault(); } }
    public SearchResults ResultsCache { get { return _searchResults; } }
    public int SearchUntilDepth { get { return _game.Turn.StepCount + _p.SearchDepth; } }

    public SearchStatistics GetSearchStatistics()
    {
      return new SearchStatistics
        {
          NodeCount = GetSearchTreeSize(),
          NumOfWorkersCreated = _numWorkersCreated,
          SubtreesPrunned = _subtreesPrunned
        };
    }

    private NodeCount GetSearchTreeSize()
    {
      var count = new NodeCount();
      _root.CountNodes(count);
      return count;
    }

    public int GetCurrentDepthInSteps(int currentStepCount)
    {
      return currentStepCount - _game.Turn.StepCount;
    }

    private int GetCurrentDepthInStateTransitions(int stateCount)
    {
      return stateCount - _game.Turn.StateCount;
    }

    private SearchWorker GetWorker(object id)
    {
      lock (_workersLock)
      {
        return _workers[id];
      }
    }

    public void Start(ISearchNode searchNode)
    {
      // Lock original changer tracker. 
      // So we are sure that original game state stays intact.
      // This is usefull for debuging state copy issues.      
      _game.ChangeTracker.Lock();

      // Both original and copied tracker will be enabled,
      // but only the copy is unlocked and can track state.
      _game.ChangeTracker.Enable();

      // Copy game state,
      var searchNodeCopy = new CopyService().CopyRoot(searchNode);

      // create the first worker
      var worker = CreateWorker(_root, searchNodeCopy.Game);

      // and start the search.
      worker.StartSearch(searchNodeCopy);

      RemoveWorker(worker);

      _game.ChangeTracker.Disable();
      _game.ChangeTracker.Unlock();

      _root.EvaluateSubtree();

      GC.Collect();
    }

    public void EvaluateNode(ISearchNode searchNode)
    {
      var worker = GetWorker(searchNode.Game);      
      worker.Evaluate(searchNode);
    }

    private SearchWorker CreateWorker(InnerResult rootResult, Game game)
    {
      var worker = new SearchWorker(this, rootResult, game, _searchResults);

      lock (_workersLock)
      {
        _workers.Add(worker.Id, worker);
        _numWorkersCreated++;
      }

      return worker;
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
      var depth = GetCurrentDepthInStateTransitions(node.Game.Turn.StateCount);
      return (_workers.Count < maxWorkers && depth == 0 && moveIndex > 0);
    }

    private bool MultiThreadedStrategy2(ISearchNode node, int moveIndex)
    {
      const int maxWorkers = 6;
      var depth = GetCurrentDepthInStateTransitions(node.Game.Turn.StateCount);
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