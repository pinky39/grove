namespace Grove.Core.Cards.Effects
{
  public class DiscardHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.DiscardHand();
    }
  }
}