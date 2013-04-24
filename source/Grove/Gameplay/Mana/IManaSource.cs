namespace Grove.Gameplay.Mana
{
  using System.Collections.Generic;

  public interface IManaSource
  {        
    bool CanActivate();
    IEnumerable<ManaUnit> PayActivationCost();        
  }
}