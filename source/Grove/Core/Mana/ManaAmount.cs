namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;

  public abstract class ManaAmount : IEnumerable<SingleColorManaAmount> {
    
    public abstract int Converted { get; }
    public abstract int Generic { get; }

    public abstract HashSet<int> Colors { get; }
    public abstract ManaAmount Add(ManaAmount amount);
    public abstract ManaAmount Remove(ManaAmount amount);

    public abstract IEnumerator<SingleColorManaAmount> GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}