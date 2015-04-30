namespace Grove.Effects
{
  public class ChangeLife : Effect
  {    
    private readonly DynParam<int> _amount;
    private readonly DynParam<Player> _whos;

    private ChangeLife() {}    
    
    public ChangeLife(DynParam<int> amount, DynParam<Player> whos)
    {
      _amount = amount;
      _whos = whos;

      RegisterDynamicParameters(amount, whos);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      if (_amount.Value >= 0)
        return 0;

      return _whos.Value == player ? -_amount.Value : 0;
    }

    protected override void ResolveEffect()
    {
      _whos.Value.Life += _amount.Value;
    }
  }
}