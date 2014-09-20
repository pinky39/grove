namespace Grove.Effects
{
  public class TapTargets : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().Tap();
      }
    }
  }
}