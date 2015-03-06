namespace Grove.Effects
{
  public class PutTargetsOnTopOfLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().PutOnTopOfLibrary();
      }
    }
  }
}