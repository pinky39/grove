namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;

  public class ZeroManaAmount : ManaAmount, IEnumerable
  {
    public override IEnumerator<SingleColorManaAmount> GetEnumerator()
    {
      yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public override int Converted { get { return 0; } }
    public override int Generic { get { return 0; } }
    public override HashSet<int> Colors { get { return new HashSet<int>();} }

    public override ManaAmount Add(ManaAmount amount)
    {
      return amount;
    }

    public override ManaAmount Remove(ManaAmount amount)
    {
      return Mana.Zero;
    }
  }
}