namespace Grove.Core.Effects
{
  using System;

  public class GainLifeEqualToTargetCreaturePower : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.Life += Target.Card().Power.Value;
    }
  }
}