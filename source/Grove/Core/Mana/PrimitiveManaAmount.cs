namespace Grove.Core.Mana
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class PrimitiveManaAmount : IManaAmount
  {
    private readonly List<ManaUnit> _amount = new List<ManaUnit>(10);

    public PrimitiveManaAmount(IEnumerable<ManaUnit> amount)
    {
      _amount.AddRange(amount);
    }

    public PrimitiveManaAmount(ManaUnit mana)
    {
      _amount.Add(mana);
    }

    public IEnumerator<ManaUnit> GetEnumerator()
    {
      return _amount.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted { get { return _amount.Count; } }

    public bool Has(ManaUnit mana)
    {
      return _amount.Any(m => m.HasColor(mana.Colors));
    }

    public ManaUnit First { get { return _amount[0]; } }

    public bool IsColorless { get { return _amount.All(x => x.IsColorless); } }

    public override string ToString()
    {
      return ManaAmount.ToString(this);
    }
  }
}