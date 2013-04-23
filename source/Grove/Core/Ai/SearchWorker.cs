namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Infrastructure;
  using log4net;

  public class SearchWorker : GameObject
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof (SearchWorker));
    private readonly Trackable<int> _moveIndex = new Trackable<int>();
    private readonly Trackable<InnerResult> _parentResult;
    private readonly ISearchNode _root;
    private readonly Search _search;
    private readonly SearchResults _searchResults;    
    private int _subTreesPrunned;

    private SearchWorker() {}

    public SearchWorker(Search search, SearchResults searchResults, ISearchNode rootNode)
    {
      _search = search;
      _searchResults = searchResults;

      _root = new CopyService().CopyRoot(rootNode);
      Game = _root.Game;      

      var innerResult = new InnerResult(
        Game.CalculateHash(), 
        _root.Controller.IsMax, 
        search.GetSearchDepthInSteps(Game.Turn.StepCount));

      _parentResult = new Trackable<InnerResult>(innerResult);
      _parentResult.Initialize(ChangeTracker);
      _moveIndex.Initialize(ChangeTracker);

      Debug("Created");
    }

    public object Id { get { return Game; } }
    
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

      if (_search.MaxDepth < Turn.StepCount)
      {
        var leafResult = new LeafResult(
          Game.Score, 
          Search.GetSearchDepthInSteps(Game.Turn.StepCount));
        
        ParentResult.AddChild(ResultIndex, leafResult);

        Game.Stop();
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
      Game.Stop();
      return;
    }

    private void EvaluateMove(ISearchNode searchNode)
    {
      Debug("{0} evaluating the only move", searchNode);

      searchNode.SetResult(0);
      Game.Simulate();
    }

    private void EvaluateMove(int moveIndex, ISearchNode searchNode, InnerResult parentResult)
    {
      Debug("{0} start eval move {1}", searchNode, moveIndex);

      var snaphost = Game.Save();
      searchNode.SetResult(moveIndex);

      // set globals for this branch
      ParentResult = parentResult;
      ResultIndex = moveIndex;

      Game.Simulate();

      // save the score before state restore            
      var score = Game.Score;

      Game.Restore(snaphost);

      // no children were added on this branch because the game has finished 
      // add a leaf node so the tree is complete
      if (!parentResult.HasChildrenWithIndex(moveIndex))
      {
        var leafResult = new LeafResult(score, 
          Search.GetSearchDepthInSteps(Game.Turn.StepCount));
        
        parentResult.AddChild(moveIndex, leafResult);
      }

      Debug("{0} stop eval move {1}", searchNode, moveIndex);
    }

    private void EvaluateMoves(ISearchNode searchNode)
    {
      InnerResult result;

      var statehash = Game.CalculateHash();
            

      Debug("state {0}, evaluating moves of node {1}", statehash, searchNode);

      var isCached = _searchResults.NewResult(
        statehash,
        searchNode.Controller.IsMax,
        Search.GetSearchDepthInSteps(Game.Turn.StepCount),
        out result);

      ParentResult.AddChild(ResultIndex, result);

      if (!isCached)
      {
        var tasks = new List<Task>();

        for (var i = 0; i < searchNode.ResultCount; i++)
        {
          var task = _search.ExecuteTask(
            parentWorker: this,
            parentNode: searchNode,
            moveIndex: i,
            action: (worker, node, moveIndex) => { if (worker != null) worker.EvaluateMove(moveIndex, node, result); });

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