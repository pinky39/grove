namespace Grove.Core.Mana
{
  using System.Collections.Generic;

  public interface IManaSource
  {        
    bool CanActivate();
    IEnumerable<ManaUnit> PayActivationCost();        
  }
}