namespace Grove.Core.Details.Cards.Effects
{
  using Ai;
  using Modifiers;

  public class DealDamageToActivePlayer : Effect
  {
    public Value Amount { get; set; }

    public override int CalculatePlayerDamage(IPlayer player)
    {
      return Amount.GetValue(X);
    }

    public override int CalculateCreatureDamage(Card creature)
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