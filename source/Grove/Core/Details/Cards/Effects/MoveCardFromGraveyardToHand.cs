namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class MoveCardFromGraveyardToHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.MoveCardFromGraveyardToHand(Target().Card());      
    }
  }
}