namespace Grove.Gameplay.ManaHandling
{
  using System;
  using System.Collections;
  using System.Collections.Generic;

  public class ZeroManaAmount : IManaAmount
  {
    public IEnumerator<SingleColorManaAmount> GetEnumerator()
    {
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted { get { return 0; } }
    public HashSet<int> Colors { get { return new HashSet<int>();} }

    public IManaAmount Add(IManaAmount amount)
    {
      return amount;
    }

    public IManaAmount Remove(IManaAmount amount)
    {
      return Mana.Zero;
    }
  }
}