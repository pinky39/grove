namespace Grove.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class NodeCount
  {
    private readonly Dictionary<int, int> _countByStep = new Dictionary<int, int>();
    
    public int this[int step]
    {
      get
      {
        if (!_countByStep.ContainsKey(step))
        {
          _countByStep[step] = 0;
          return 0;
        }

        return _countByStep[step];
      }
      set
      {
        _countByStep[step] = value;
      }
    }

    public int Total
    {
      get { return _countByStep.Sum(x => x.Value); }
    }

    public override string ToString()
    {
      var sb = new StringBuilder();

      sb.AppendFormat("{0} (", Total);      
      
      foreach (var keyValuePair in _countByStep.OrderBy(x => x.Key))
      {
        sb.AppendFormat("{0}:{1};", keyValuePair.Key, keyValuePair.Value);
      }

      sb.Append(")");

      return sb.ToString();
    }
  }
}