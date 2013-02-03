namespace Grove.Core.Effects
{
  using Targeting;

  public class EnchantOwnerWithTarget : Effect
  {
    protected override void ResolveEffect()
    {
      var enchantment = Target.Card();
      Source.OwningCard.EnchantWithoutPayingCost(enchantment);
    }
  }
}