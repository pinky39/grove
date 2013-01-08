namespace Grove.Core.Mana
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class AggregateManaAmount : IManaAmount
  {
    private readonly List<IManaAmount> _amounts = new List<IManaAmount>(10);
    private int? _converted;

    public AggregateManaAmount(params IManaAmount[] amounts)
    {
      _amounts.AddRange(amounts);
    }

    public AggregateManaAmount(IEnumerable<IManaAmount> amounts)
    {
      _amounts.AddRange(amounts);
    }

    public IEnumerator<ManaUnit> GetEnumerator()
    {
      return _amounts.SelectMany(amount => amount).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted
    {
      get
      {
        return _converted ?? (
          _converted = _amounts.Sum(x => x.Converted)).Value;
      }
    }

    public bool Has(ManaUnit mana)
    {
      return _amounts.Any(amount => amount.Has(mana));
    }

    public ManaUnit First { get { return this.First(); } }

    public bool IsColorless { get { return _amounts.All(amount => amount.IsColorless); } }

    public override string ToString()
    {
      return ManaAmount.ToString(this);
    }
  }
}