namespace Grove.Gameplay.Effects
{
  using Artifical;

  public class Regenerate : Effect
  {
    public Regenerate()
    {
      Category = EffectCategories.Protector;
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.CanRegenerate = true;
    }
  }
}