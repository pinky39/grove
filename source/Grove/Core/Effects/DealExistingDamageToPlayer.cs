namespace Grove.Effects
{
  public class DealExistingDamageToPlayer : Effect
  {
    private readonly DynParam<Damage> _damage;
    private readonly DynParam<Player> _player;

    private DealExistingDamageToPlayer() {}

    public DealExistingDamageToPlayer(DynParam<Damage> damage, DynParam<Player> player)
    {
      _damage = damage;
      _player = player;

      RegisterDynamicParameters(damage, _player);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == _player.Value ? _damage.Value.Amount : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      _player.Value.ReceiveDamage(_damage.Value);
    }
  }
}