namespace Grove.Gameplay.Effects
{
  using Damage;
  using Modifiers;

  public class DealDamageToPlayer : Effect
  {
    private readonly Value _amount;
    private readonly DynParam<Player> _player;

    private DealDamageToPlayer() {}

    public DealDamageToPlayer(Value amount, DynParam<Player> player)
    {
      _amount = amount;
      _player = player;

      RegisterDynamicParameters(player);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == _player.Value ? _amount.GetValue(X) : 0;
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
        changeTracker: ChangeTracker
        );

      _player.Value.DealDamage(damage);
    }
  }
}