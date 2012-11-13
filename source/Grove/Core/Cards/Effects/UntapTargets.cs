namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

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