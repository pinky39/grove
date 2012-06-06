namespace Grove.Core
{
  using System.Collections.Generic;

  public interface IManaAmount : IEnumerable<Mana>
  {
    int Converted { get; }
  }
}