namespace Grove
{
  using System.Collections.Generic;

  public interface IManaAmount : IEnumerable<SingleColorManaAmount>
  {
    int Converted { get; }

    HashSet<int> Colors { get; }
    IManaAmount Add(IManaAmount amount);
    IManaAmount Remove(IManaAmount amount);
  }
}