namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class InnerResult : ISearchResult
  {
    private readonly object _access = new object();
    private readonly List<Tuple<int, ISearchResult>> _children = new List<Tuple<int, ISearchResult>>(10);
    private readonly bool _isMax;
    private int? _bestMove;
    private int? _bestScore;
    private int? _shortestPath;

    public InnerResult(bool isMax)
    {
      _isMax = isMax;
    }

    public int BestMove
    {
      get
      {
        if (_bestMove.HasValue)
        {
          return _bestMove.Value;
        }

        Evaluate();

        return _bestMove.Value;
      }
    }

    public int ShortestPath
    {
      get
      {
        if (_shortestPath.HasValue)
          return _shortestPath.Value;

        Evaluate();

        return _shortestPath.Value;
      }
    }

    public int Score
    {
      get
      {
        if (_bestScore.HasValue)
          return _bestScore.Value;

        Evaluate();

        return _bestScore.Value;
      }
    }


    public void AddChild(int moveIndex, ISearchResult resultNode)
    {
      lock (_access)
      {
        _children.Add(
          new Tuple<int, ISearchResult>(moveIndex, resultNode));
      }
    }

    private void Evaluate()
    {
      lock (_access)
      {
        _shortestPath = _children.Min(x => x.Item2.ShortestPath) + 1;
        
        var best = _isMax
          ? _children.OrderByDescending(x => x.Item2.Score).ThenBy(x => x.Item2.ShortestPath).First()
          : _children.OrderBy(x => x.Item2.Score).ThenBy(x => x.Item2.ShortestPath).First();

        _bestMove = best.Item1;
        _bestScore = best.Item2.Score;
      }
    }
  }
}