namespace Grove.Gameplay
{
  using System.Collections.Generic;

  public interface IManaSource
  {
    bool CanActivate();
    void PayActivationCost();
    IEnumerable<ManaUnit> GetUnits();
  }
}