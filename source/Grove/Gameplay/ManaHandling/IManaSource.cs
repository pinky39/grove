namespace Grove.Gameplay.ManaHandling
{
  using System.Collections.Generic;

  public interface IManaSource
  {
    bool CanActivate();
    void PayActivationCost();
    IEnumerable<ManaUnit> GetUnits();
  }
}