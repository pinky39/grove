namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;

  public class TapTarget : Effect
  {
    protected override void ResolveEffect()
    {
      Target().Card().Tap();
    }
  }
}