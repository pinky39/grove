namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class TapTargetCreature : Effect
  {
    protected override void ResolveEffect()
    {
      Target().Card().Tap();
    }
  }
}