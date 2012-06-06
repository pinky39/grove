namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;

  public class InnerResult : ISearchResult
  {
    private readonly object _access = new object();
    private readonly List<Child> _children = new List<Child>(10);
    private readonly bool _isMax;
    private int? _bestMove;
    private int? _bestScore;
    private bool _isVisited;
    private int? _shortestPath;

    public InnerResult(bool isMax)
    {
      _isMax = isMax;
    }

    public int? BestMove
    {
      get
      {
        return _bestMove;
      }
    }

    public int? ShortestPath
    {
      get
      {        
        return _shortestPath;        
      }
    }

    public int? Score
    {
      get
      {
        return _bestScore;
      }
    }


    public void Visit()
    {
      if (_isVisited)
        return;
      
      _isVisited = true;

      foreach (var child in _children)
      {
        child.Result.Visit();
      }

      _shortestPath = _children.Min(x => x.Result.ShortestPath) + 1;

      var best = _isMax
          ? _children.OrderByDescending(x => x.Result.Score).ThenBy(x => x.Result.ShortestPath).First()
          : _children.OrderBy(x => x.Result.Score).ThenBy(x => x.Result.ShortestPath).First();

      _bestMove = best.MoveIndex;
      _bestScore = best.Result.Score;
    }

    public void AddChild(int moveIndex, ISearchResult resultNode)
    {
      lock (_access)
      {
        _children.Add(
          new Child {MoveIndex = moveIndex, Result = resultNode});
      }
    }   

    private class Child
    {
      public int MoveIndex;
      public ISearchResult Result;
    }
  }
}