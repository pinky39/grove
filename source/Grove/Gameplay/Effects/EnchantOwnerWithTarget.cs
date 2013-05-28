namespace Grove.Gameplay.Effects
{
  using System;
  using Targeting;

  [Serializable]
  public class EnchantOwnerWithTarget : Effect
  {
    protected override void ResolveEffect()
    {
      var enchantment = Target.Card();
      enchantment.EnchantWithoutPayingCost(Source.OwningCard);
    }
  }
}