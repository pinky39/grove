namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class EnchantOwnerWithTarget : Effect
  {
    public override bool NeedsTargets
    {
      get { return true; }
    }
    
    protected override void ResolveEffect()
    {            
      var enchantment = Target().Card();
      Source.OwningCard.EnchantWithoutPayingTheCost(enchantment);      
    }
  }
}