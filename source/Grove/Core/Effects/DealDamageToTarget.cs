namespace Grove.Core.Effects
{
  using Modifiers;

  public class DealDamageToTarget : Effect
  {
    public Value Amount { get; set; }
    
    public override void Resolve()
    {
      var damageSource = Source.OwningCard;
      var target = Target;

      target.DealDamage(damageSource, Amount.GetValue(X), isCombat: false);
    }

    public override string ToString()
    {
      return GetType().Name;
    }    
  }
}