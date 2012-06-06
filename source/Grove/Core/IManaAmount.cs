namespace Grove.Core
{
  using System.Collections.Generic;

  public interface IManaAmount : IEnumerable<Mana>
  {
    int Converted { get; }
    bool Has(Mana mana);
    Mana First { get; }
    bool IsColorless { get; }
  }
}