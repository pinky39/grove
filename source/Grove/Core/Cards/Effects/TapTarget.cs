namespace Grove.Core.Cards.Effects
{
  using Targeting;

  public class TapTarget : Effect
  {
    protected override void ResolveEffect()
    {
      Target().Card().Tap();
    }
  }
}