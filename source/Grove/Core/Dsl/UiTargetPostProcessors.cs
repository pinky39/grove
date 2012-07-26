namespace Grove.Core.Dsl
{
  using System;
  using Targeting;

  public class UiTargetPostProcessors
  {
    private readonly DistributeDamage.IFactory _distributeDamage;

    public UiTargetPostProcessors(DistributeDamage.IFactory distributeDamage)
    {
      _distributeDamage = distributeDamage;
    }

    public Func<UiTargetPostprocessor> DistributeDamage(int amount)
    {
      return () => _distributeDamage.Create(amount);
    }
  }
}