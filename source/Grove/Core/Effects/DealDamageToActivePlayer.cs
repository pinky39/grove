namespace Grove.Core.Effects
{
  using Modifiers;

  public class DealDamageToActivePlayer : Effect
  {
    private readonly Value _amount;

    private DealDamageToActivePlayer()
    {      
    }
    
    public DealDamageToActivePlayer(Value amount)
    {
      _amount = amount;
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return _amount.GetValue(X);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      var damage = new Damage(
        source: Source.OwningCard,
        amount: _amount.GetValue(X),
        isCombat: false,
        changeTracker: Game.ChangeTracker
        );

      Core.Players.Active.DealDamage(damage);
    }
  }
}