namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class GainLifeEqualToTargetPower : Effect
  {
    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      Controller.Life += Target().Card().Power.Value;
    }
  }
}