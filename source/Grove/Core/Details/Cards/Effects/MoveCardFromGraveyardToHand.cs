namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class MoveCardFromGraveyardToHand : Effect
  {
    protected override void ResolveEffect()
    {
      Target().Card().ReturnToHand();            
    }
  }
}