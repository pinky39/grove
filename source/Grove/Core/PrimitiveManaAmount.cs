namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class PrimitiveManaAmount : IManaAmount
  {
    private readonly List<Mana> _amount = new List<Mana>(10);

    public PrimitiveManaAmount(IEnumerable<Mana> amount)
    {
      _amount.AddRange(amount);
    }

    public PrimitiveManaAmount(Mana mana)
    {
      _amount.Add(mana);
    }

    #region IManaAmount Members

    public IEnumerator<Mana> GetEnumerator()
    {
      return _amount.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted
    {
      get { return _amount.Count; }
    }

    public bool Has(Mana mana)
    {
      return _amount.Any(m => m.HasColor(mana.Colors));
    }

    public Mana First
    {
      get { return _amount[0]; }
    }

    public bool IsColorless
    {
      get { return _amount.All(x => x.IsColorless); }
    }

    #endregion
  }
}