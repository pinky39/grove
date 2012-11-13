namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

  public class GainLifeEqualToTargetPower : Effect
  {    
    protected override void ResolveEffect()
    {
      Controller.Life += Target().Card().Power.Value;
    }
  }
}