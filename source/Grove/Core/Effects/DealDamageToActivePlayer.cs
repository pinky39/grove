namespace Grove.Core.Effects
{
  using Modifiers;

  public class DealDamageToActivePlayer : Effect
  {
    public Value Amount { get; set; }    
    
    protected override void ResolveEffect()
    {
      var damageSource = Source.OwningCard;      
      Players.Active.DealDamage(damageSource, Amount.GetValue(X));
    }
  }
}