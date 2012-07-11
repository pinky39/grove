namespace Grove.Core.Controllers.Results
{
  using System.Collections.Generic;
  using System.Linq;
  using Details.Combat;
  using Infrastructure;

  [Copyable]
  public class DamageDistribution
  {
    private readonly Dictionary<Blocker, int> _distribution = new Dictionary<Blocker, int>();

    public int this[Blocker blocker]
    {
      get
      {
        var assigned = 0;
        _distribution.TryGetValue(blocker, out assigned);
        return assigned;
      }
    }

    public int Total { get { return _distribution.Sum(x => x.Value); } }

    public void Assign(Blocker blocker, int amount)
    {
      var assigned = 0;
      _distribution.TryGetValue(blocker, out assigned);
      _distribution[blocker] = assigned + amount;
    }
  }
}