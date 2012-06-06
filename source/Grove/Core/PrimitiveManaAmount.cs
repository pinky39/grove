namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;

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

    #endregion
  }
}