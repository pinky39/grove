namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class PutTargetToBattlefield : Effect
  {
    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      Controller.PutCardToBattlefield(Target().Card());
    }
  }
}