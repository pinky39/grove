namespace Grove.Core.Effects
{
  using Ai;
  using Modifiers;

  public class DealDamageToActivePlayer : Effect, IDamageDealing
  {
    public Value Amount { get; set; }

    int IDamageDealing.PlayerDamage(Player player)
    {
      return Amount.GetValue(X);
    }

    int IDamageDealing.CreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      var damageSource = Source.OwningCard;
      Players.Active.DealDamage(damageSource, Amount.GetValue(X));
    }
  }
}