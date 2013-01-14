namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;

  public class EnchantOwnerWithTarget : Effect
  {    
    protected override void ResolveEffect()
    {            
      var enchantment = Target().Card();
      Source.OwningCard.EnchantWith(enchantment);      
    }
  }
}