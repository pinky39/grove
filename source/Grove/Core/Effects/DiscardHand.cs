namespace Grove.Core.Effects
{
  public class DiscardHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.DiscardHand();
    }
  }
}