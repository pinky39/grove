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
    private readonly SearchResults _searchResults;

    private SearchWorker() {}

    public SearchWorker(InnerResult rootResult, Game game, SearchResults searchResults)
    {      
      Game = game;
      
      _searchResults = searchResults;            
      _parentResult = new Trackable<InnerResult>(rootResult).Initialize(ChangeTracker);      
      _moveIndex.Initialize(ChangeTracker);      
    }

    public object Id { get { return Game; } }
    
    private InnerResult ParentResult { get { return _parentResult.Value; } set { _parentResult.Value = value; } }
    private int ResultIndex { get { return _moveIndex.Value; } set { _moveIndex.Value = value; } }
    public int SubTreesPrunned { get; private set; }

    public override string ToString()
    {
      return GetHashCode().ToString();
    }

    private int SearchDepth { get { return Search.GetSearchDepthInSteps(Game.Turn.StepCount); } }

    private void Debug(string format, params object[] args)
    {
#if DEBUG
      Log.DebugFormat(string.Format("Worker {0}: {1}", this, format), args);
#endif
    }

    public void StartSearch(ISearchNode root)
    {
      Debug("Search started, evaluating node: {0}", root);
      EvaluateBranches(root);
    }

    public void Evaluate(ISearchNode node)
    {      
      Debug("Evaluating node: {0}", node);      

      if (Search.MaxDepth < Turn.StepCount)
      {
        // We have reached our intended search depth.
        // We record the final score and stop the game.        
        var leafResult = new LeafResult(Game.Score, SearchDepth);        
        ParentResult.AddChild(ResultIndex, leafResult);
        Game.Stop();
        return;
      }   
            
      if (node.ResultCount == 1)
      {
        // Set the only result, nothing to evaluate here.
        node.SetResult(0);
        return;
      }
      
      // Evaluate each child branch.     
      EvaluateBranches(node);

      // By evaluating each branch we have already traversed the
      // entire subtree, so we stop the game and yield control to
      // an upper level.
      Game.Stop();
      return;
    }

    public void EvaluateBranch(int index, ISearchNode searchNode, InnerResult parentResult)
    {
      Debug("{0} start eval move {1}", searchNode, index);

      var snaphost = Game.CreateSnapshot();
      
      searchNode.SetResult(index);      
      ParentResult = parentResult;
      ResultIndex = index;

      Game.Simulate();
      
      
      if (parentResult.HasChildrenWithIndex(index) == false)
      {
        // no children were added on this branch because the game has finished 
        // add a leaf node so the tree is complete        
        var leafResult = new LeafResult(Game.Score, SearchDepth);        
        parentResult.AddChild(index, leafResult);
      }

      Game.Restore(snaphost);

      Debug("{0} stop eval move {1}", searchNode, index);
    }

    private void EvaluateBranches(ISearchNode searchNode)
    {
      InnerResult rootResult;

      var statehash = Game.CalculateHash();
            

      Debug("state {0}, evaluating moves of node {1}", statehash, searchNode);

      var isCached = _searchResults.NewResult(
        statehash,
        searchNode.Controller.IsMax,
        Search.GetSearchDepthInSteps(Game.Turn.StepCount),
        out rootResult);

      ParentResult.AddChild(ResultIndex, rootResult);

      if (!isCached)
      {
        var tasks = new List<Task>();

        for (var i = 0; i < searchNode.ResultCount; i++)
        {
          var task = Search.EvaluateBranch(
            worker: this,
            rootNode: searchNode,
            rootResult: rootResult,
            moveIndex: i);                

          tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
        return;
      }

      SubTreesPrunned++;
      Debug("state {0}, prunning node {1}", statehash, searchNode);
    }
  }
}