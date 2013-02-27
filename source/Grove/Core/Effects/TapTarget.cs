namespace Grove.Core.Effects
{
  using Targeting;

  public class TapTarget : Effect
  {
    protected override void ResolveEffect()
    {
      Target.Card().Tap();
    }
  }
}