namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class MoveCardFromGraveyardToHand : Effect
  {
    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      Target().Card().ReturnToHand();
    }
  }
}