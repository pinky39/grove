namespace Grove.AI
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Infrastructure;

  public class Search
  {
    private readonly Game _game;
    private readonly SearchParameters _p;
    private readonly InnerResult _root;
    private readonly SearchResults _searchResults;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly Dictionary<object, SearchWorker> _workers = new Dictionary<object, SearchWorker>();
    private readonly object _workersLock = new object();
    private int _numWorkersCreated;
    private int _subtreesPrunned;

    public Search(SearchParameters p, Player searchingPlayer, SearchResults searchResults, Game game)
    {
      _p = p;
      _searchResults = searchResults;
      _game = game;
      _root = new InnerResult(_game.CalculateHash(), searchingPlayer.IsMax, 0);
    }

    public int WorkerCount { get { return _workers.Count; } }
    public int Result { get { return _root.BestMove.GetValueOrDefault(); } }
    public int SearchUntilDepth { get { return _game.Turn.StepCount + _p.SearchDepth; } }
    public TimeSpan Duration { get { return _stopwatch.Elapsed; } }

    public int GetCurrentDepthInSteps(int currentStepCount)
    {
      return currentStepCount - _game.Turn.StepCount;
    }

    public SearchStatistics Start(ISearchNode searchNode)
    {
      _stopwatch.Start();

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
      _stopwatch.Stop();

      return GetSearchStatistics();
    }

    public void EvaluateNode(ISearchNode searchNode)
    {
      var worker = GetWorker(searchNode.Game);
      worker.Evaluate(searchNode);
    }

    public Task EvaluateBranch(SearchWorker worker, ISearchNode rootNode, InnerResult rootResult, int moveIndex)
    {
      var shouldCreateNewWorker = IsItFeasibleToCreateNewWorker(rootNode, moveIndex);

      if (shouldCreateNewWorker)
      {
        rootNode = new CopyService().CopyRoot(rootNode);
        worker = CreateWorker(rootResult, rootNode.Game);

        var task = Task.Factory.StartNew(() =>
          {
            worker.EvaluateBranch(moveIndex, rootNode, rootResult);
            RemoveWorker(worker);
          }, TaskCreationOptions.PreferFairness);

        return task;
      }
      worker.EvaluateBranch(moveIndex, rootNode, rootResult);
      return null;
    }

    private SearchStatistics GetSearchStatistics()
    {
      return new SearchStatistics
        {
          SearchTreeSize = GetSearchTreeSize(),
          NumOfWorkersCreated = _numWorkersCreated,
          SubtreesPrunned = _subtreesPrunned,
          Elapsed = _stopwatch.Elapsed
        };
    }

    private TreeSize GetSearchTreeSize()
    {
      var treeSize = new TreeSize();
      _root.CountNodes(treeSize);
      return treeSize;
    }


    private SearchWorker GetWorker(object id)
    {
      lock (_workersLock)
      {
        return _workers[id];
      }
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
      return _p.SearchPartitioningStrategy(
        new SearchPartitioningStrategyParameters(node, moveIndex, this, _game));
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
  }
}