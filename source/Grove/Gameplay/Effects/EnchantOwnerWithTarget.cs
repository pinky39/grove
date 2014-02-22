namespace Grove.Gameplay.Effects
{
  public class EnchantOwnerWithTarget : Effect
  {
    protected override void ResolveEffect()
    {
      var enchantment = Target.Card();
      enchantment.EnchantWithoutPayingCost(Source.OwningCard);
    }
  }
}