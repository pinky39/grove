namespace Grove.Core.Details.Cards.Effects
{
  using Ai;
  using Modifiers;

  public class DealDamageToActivePlayer : Effect, IDamageDealing
  {
    public Value Amount { get; set; }

    int IDamageDealing.PlayerDamage(IPlayer player)
    {
      return Amount.GetValue(X);
    }

    int IDamageDealing.CreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      var damage = new Damage(
        source: Source.OwningCard,
        amount: Amount.GetValue(X),
        isCombat: false,
        changeTracker: Game.ChangeTracker
        );

      Players.Active.DealDamage(damage);
    }
  }
}