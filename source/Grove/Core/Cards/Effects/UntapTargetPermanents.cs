namespace Grove.Core.Cards.Effects
{
  using Targeting;

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