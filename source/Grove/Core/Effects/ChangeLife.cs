namespace Grove.Effects
{
  public class ChangeLife : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly bool _opponents;
    private readonly bool _targetPlayers;
    private readonly bool _yours;
    private Player _you;

    private ChangeLife() {}

    public ChangeLife(DynParam<int> amount, bool yours = false, bool opponents = false, bool targetPlayers = false)
    {
      _amount = amount;

      _yours = yours;
      _opponents = opponents;
      _targetPlayers = targetPlayers;

      RegisterDynamicParameters(amount);
    }

    protected override void Initialize()
    {
      _you = Source.OwningCard.Controller;
    }

    public override int CalculatePlayerDamage(Player player)
    {
      if (_amount.Value >= 0)
        return 0;


      if (_yours && player == _you)
      {
        return _amount.Value;
      }

      if (_opponents && player == _you.Opponent)
      {
        return _amount.Value;
      }

      if (player == Target)
      {
        return _amount.Value;
      }

      return 0;
    }

    protected override void ResolveEffect()
    {
      if (_yours)
      {
        _you.Life += _amount.Value;
      }

      if (_opponents)
      {
        _you.Opponent.Life += _amount.Value;
      }

      if (_targetPlayers)
      {
        Target.Player().Life += _amount.Value;
      }
    }
  }
}