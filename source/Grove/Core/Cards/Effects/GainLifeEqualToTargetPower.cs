namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class GainLifeEqualToTargetPower : Effect
  {    
    protected override void ResolveEffect()
    {
      Controller.Life += Target().Card().Power.Value;
    }
  }
}