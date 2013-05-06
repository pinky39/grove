namespace Grove.Gameplay.ManaHandling
{
  using System.Collections.Generic;

  public interface IManaSource
  {
    bool CanActivate();
    IEnumerable<ManaUnit> PayActivationCost();
  }
}