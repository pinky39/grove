namespace Grove.Core.Effects
{
  using Modifiers;

  public class DealDamageToTarget : Effect, IDamageDealing
  {
    public Value Amount { get; set; }

    public int PlayerDamage(Player player)
    {
      return 0;
    }

    public int CreatureDamage(Card creature)
    {
      return creature == Target ? Amount.GetValue(X) : 0;
    }

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