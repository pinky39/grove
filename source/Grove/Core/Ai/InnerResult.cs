namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class InnerResult : ISearchResult
  {
    private readonly object _access = new object();
    private readonly List<Edge> _children = new List<Edge>(10);
    private readonly int _id;
    private readonly bool _isMax;
    private Edge _bestEdge;
    private bool _isVisited;

    public InnerResult(int id, bool isMax)
    {
      _id = id;
      _isMax = isMax;
    }

    public int? BestMove { get { return _bestEdge != null ? _bestEdge.MoveIndex : (int?) null; } }
    public int? Score { get; set; }

    public void EvaluateSubtree()
    {
      _isVisited = true;

      foreach (var child in _children.ToList())
      {
        if (!child.Result.Score.HasValue)
        {
          if (child.Result.IsVisited)
          {
            // cycle detected remove this child
            _children.Remove(child);
            continue;
          }

          // calculate child score
          child.Result.EvaluateSubtree();
        }
      }

      if (_children.Count == 0)
        return;
      
      Score = _isMax 
        ? _children.Max(x => x.Result.Score) 
        : _children.Min(x => x.Result.Score);

      if (Score != null)
      {
        _bestEdge = _children.First(x => x.Result.Score == Score);
      }
    }

    public bool IsVisited { get { return _isVisited; } }

    public StringBuilder OutputBestPath(StringBuilder sb = null)
    {
      sb = sb ?? new StringBuilder();
      sb.AppendFormat("{0}, ", _id);
      return _bestEdge.Result.OutputBestPath(sb);
    }

    public void AddChild(int moveIndex, ISearchResult resultNode)
    {
      lock (_access)
      {
        _children.Add(
          new Edge {MoveIndex = moveIndex, Result = resultNode});
      }
    }

    private class Edge
    {
      public int MoveIndex;
      public ISearchResult Result;
    }
  }
}