namespace Grove.Core.Effects
{
  public class GainLifeEqualToTargetPower : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.Life += Target().Card().Power.Value;
    }
  }
}