namespace Grove.Core.Details.Cards.Effects
{
  public class Regenerate : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.CanRegenerate = true;
    }
  }
}