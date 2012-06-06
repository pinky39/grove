namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;

  public class ZeroManaAmount : IManaAmount
  {
    #region IManaAmount Members

    public IEnumerator<Mana> GetEnumerator()
    {
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted
    {
      get { return 0; }
    }

    public bool Has(Mana mana)
    {
      return false;
    }

    public Mana First
    {
      get { return null; }
    }

    public bool IsColorless
    {
      get { return true; }
    }

    #endregion
  }
}