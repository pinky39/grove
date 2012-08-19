namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class UntapTargets : Effect
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