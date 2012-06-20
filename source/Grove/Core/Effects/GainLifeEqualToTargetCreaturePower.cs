namespace Grove.Core.Effects
{
  public class GainLifeEqualToTargetCreaturePower : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.Life += Target.Card().Power.Value;
    }
  }
}