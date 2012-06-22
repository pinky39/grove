namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class InnerResult : ISearchResult
  {
    private readonly object _access = new object();
    private readonly List<Child> _children = new List<Child>(10);
    private readonly int _id;
    private readonly bool _isMax;
    private Child _bestChild;
    private bool _isVisited;

    public InnerResult(int id, bool isMax)
    {
      _id = id;
      _isMax = isMax;
    }

    public int? BestMove { get { return _bestChild == null ? (int?) null : _bestChild.MoveIndex; } }
    public int? Score { get { return _bestChild == null ? null : _bestChild.Result.Score; } }

    public void EvaluateSubtree()
    {
      if (_isVisited)
        return;

      _isVisited = true;

      foreach (var child in _children)
      {
        child.Result.EvaluateSubtree();
      }

      _bestChild = _isMax
        ? _children.OrderByDescending(x => x.Result.Score).First()
        : _children.OrderBy(x => x.Result.Score).First();
    }

    public StringBuilder OutputBestPath(StringBuilder sb = null)
    {
      sb = sb ?? new StringBuilder();
      sb.AppendFormat("{0}, ", _id);
      return _bestChild.Result.OutputBestPath(sb);
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