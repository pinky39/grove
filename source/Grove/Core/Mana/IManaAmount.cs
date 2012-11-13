namespace Grove.Core.Mana
{
  using System.Collections.Generic;

  public interface IManaAmount : IEnumerable<ManaUnit>
  {
    int Converted { get; }
    ManaUnit First { get; }
    bool IsColorless { get; }
    bool Has(ManaUnit mana);
  }
}