namespace Grove.Effects
{
  public class DealDamageToPlayer : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly DynParam<Player> _player;

    private DealDamageToPlayer() {}

    public DealDamageToPlayer(DynParam<int> amount, DynParam<Player> player)
    {
      _amount = amount;
      _player = player;

      RegisterDynamicParameters(player, amount);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == _player.Value ? _amount.Value : 0;
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.DealDamageTo(
        _amount.Value,
        _player.Value,
        isCombat: false);
    }
  }
}