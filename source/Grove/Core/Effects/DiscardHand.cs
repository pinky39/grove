namespace Grove.Effects
{
  public class DiscardHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.DiscardHand();
    }
  }
}