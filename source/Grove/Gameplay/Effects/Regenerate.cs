namespace Grove.Gameplay.Effects
{
  using System;
  using Artifical;

  [Serializable]
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