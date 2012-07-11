namespace Grove.Core.Ai
{
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

    public int? BestMove { get { return _bestEdge == null ? (int?) null : _bestEdge.MoveIndex; } }
    public int? Score { get { return _bestEdge == null ? null : _bestEdge.Result.Score; } }


    public void EvaluateSubtree()
    {
      _isVisited = true;

      foreach (var child in _children)
      {
        if (child.Result.IsVisited)
        {
          child.IsCycle = true;
          continue;
        }

        child.Result.EvaluateSubtree();
      }

      var scoredChildren = _children.Where(x => !x.IsCycle && x.Result.Score != null).ToList();

      if (scoredChildren.Count > 0)
      {
        _bestEdge = _isMax
          ? scoredChildren.OrderByDescending(x => x.Result.Score).First()
          : scoredChildren.OrderBy(x => x.Result.Score).First();
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
      public bool IsCycle;
      public int MoveIndex;
      public ISearchResult Result;
    }
  }
}