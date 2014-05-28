namespace Grove
{
  using System.Collections.Generic;

  public interface IManaSource
  {
    bool CanActivate();
    void PayActivationCost();
    Card OwningCard { get; }

    IEnumerable<ManaUnit> GetUnits();
  }
}