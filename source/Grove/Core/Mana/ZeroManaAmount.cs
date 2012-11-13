namespace Grove.Core.Mana
{
  using System.Collections;
  using System.Collections.Generic;

  public class ZeroManaAmount : IManaAmount
  {
    public IEnumerator<ManaUnit> GetEnumerator()
    {
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted { get { return 0; } }

    public bool Has(ManaUnit mana)
    {
      return false;
    }

    public ManaUnit First { get { return null; } }

    public bool IsColorless { get { return true; } }

    public override string ToString()
    {
      return "{0}";
    }
  }
}