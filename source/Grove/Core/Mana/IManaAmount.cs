namespace Grove.Core.Mana
{
  using System.Collections.Generic;

  public interface IManaAmount : IEnumerable<ManaUnit>
  {
    int Converted { get; }    
    bool IsColorless { get; }
    
    ManaUnit First { get; }
    bool Has(ManaUnit mana);
  }
}