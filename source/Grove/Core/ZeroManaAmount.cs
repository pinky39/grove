namespace Grove.Core
{
  using System.Collections;
  using System.Collections.Generic;

  public class ZeroManaAmount : IManaAmount
  {
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
  }
}