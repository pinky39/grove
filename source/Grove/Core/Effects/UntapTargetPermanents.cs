namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;

  public class UntapTargetPermanents : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var validTarget in ValidTargets)
      {
        validTarget.Card().Untap();
      }
    }
  }
}