namespace Grove.Core.Effects
{
  using System;
  using Modifiers;

  public class DealDamageToTarget : Effect
  {
    public Value Amount { get; set; }

    protected override void ResolveEffect()
    {
      var damageSource = Source.OwningCard;
      var target = Target;

      target.DealDamage(damageSource, Amount.GetValue(X));
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}