namespace Grove.Gameplay.Effects
{
  using System;
  using Artifical;

  public class RegenerateOwner : Effect
  {
    public RegenerateOwner()
    {
      Category = EffectCategories.Protector;
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.HasRegenerationShield = true;
    }
  }
}