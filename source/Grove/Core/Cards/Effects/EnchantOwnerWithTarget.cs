namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class EnchantOwnerWithTarget : Effect
  {    
    protected override void ResolveEffect()
    {            
      var enchantment = Target().Card();
      Source.OwningCard.EnchantWithoutPayingTheCost(enchantment);      
    }
  }
}