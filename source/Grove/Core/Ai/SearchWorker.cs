namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Infrastructure;
  using log4net;

  public class SearchWorker
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (SearchWorker));
    private readonly Game _game;
    private readonly Trackable<int> _moveIndex;
    private readonly Trackable<InnerResult> _parentResult;
    private readonly ISearchNode _root;
    private readonly Search _search;
    private readonly SearchResults _searchResults;
    private int _nodesSearched;
    private int _subTreesPrunned;

    public SearchWorker(
      Search search,
      SearchResults searchResults,
      ISearchNode rootNode)
    {
      _search = search;
      _searchResults = searchResults;

      _root = new CopyService().CopyRoot(rootNode);
      _game = _root.Game;
      _parentResult = new Trackable<InnerResult>(
        new InnerResult(_game.CalculateHash(), _root.Player.IsMax), _game.ChangeTracker);

      _moveIndex = new Trackable<int>(_game.ChangeTracker);

      Debug("Created");
    }

    public object Id { get { return _game; } }

    public int NodesSearched { get { return _nodesSearched; } }
    private InnerResult ParentResult { get { return _parentResult.Value; } set { _parentResult.Value = value; } }
    private int ResultIndex { get { return _moveIndex.Value; } set { _moveIndex.Value = value; } }
    public ISearchNode Root { get { return _root; } }
    public int SubTreesPrunned { get { return _subTreesPrunned; } }

    public override string ToString()
    {
      return GetHashCode().ToString();
    }

    private void Debug(string format, params object[] args)
    {
      Log.DebugFormat(
        string.Format("Worker {0}: {1}", this, format),
        args);
    }

    public void Evaluate(ISearchNode node = null)
    {
      node = node ?? _root;

      Debug("Evaluating node: {0}", node);

      _nodesSearched++;

      if (_search.MaxDepth < _game.Turn.StepCount)
      {
        ParentResult.AddChild(ResultIndex, new LeafResult(_game.Score));

        _game.Stop();
        return;
      }

      if (node.ResultCount == 1)
      {
        EvaluateMove(node);
      }
      else
      {
        EvaluateMoves(node);
      }

      if (node == _root)
      {
        return;
      }

      // at this point we have already evaluated the subtree
      // of this node, so we can stop and backtrack      
      _game.Stop();
      return;
    }

    private void EvaluateMove(ISearchNode searchNode)
    {
      Debug("{0} evaluating the only move", searchNode);

      searchNode.SetResult(0);
      _game.Simulate();

      if (_game.IsFinished)
      {
        ParentResult.AddChild(ResultIndex,
          new LeafResult(_game.Score));
      }
    }

    private void EvaluateMove(int moveIndex, ISearchNode searchNode, InnerResult parentResult)
    {
      Debug("{0} start eval move {1}", searchNode, moveIndex);

      var snaphost = _game.Save();

      searchNode.SetResult(moveIndex);
      ParentResult = parentResult;
      ResultIndex = moveIndex;

      _game.Simulate();

      if (_game.IsFinished)
      {
        ParentResult.AddChild(
          ResultIndex, new LeafResult(_game.Score));
      }

      _game.Restore(snaphost);

      Debug("{0} stop eval move {1}", searchNode, moveIndex);
    }

    private void EvaluateMoves(ISearchNode searchNode)
    {
      InnerResult result;

      var statehash = _game.CalculateHash();

      Debug("state {0}, evaluating moves of node {1}", statehash, searchNode);

      var isCached = _searchResults.NewResult(
        statehash,
        searchNode.Player.IsMax,
        out result);

      ParentResult.AddChild(ResultIndex, result);

      if (!isCached)
      {
        var tasks = new List<Task>();

        for (var i = 0; i < searchNode.ResultCount; i++)
        {
          var moveIndex = i;

          var task = _search.ExecuteTask(
            parentWorker: this,
            parentNode: searchNode,
            resultIndex: moveIndex,
            action: (worker, node) => { if (worker != null) worker.EvaluateMove(moveIndex, node, result); });

          tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
        return;
      }

      _subTreesPrunned++;
      Debug("state {0}, prunning node {1}", statehash, searchNode);
    }
  }
}