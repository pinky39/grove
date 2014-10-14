namespace Grove.Effects
{
  using System;
  using System.Linq;

  public class ChangeLife : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly bool _forOpponent;
    private readonly bool _forTargetPlayer;
    private readonly bool _forYou;
    private readonly bool _useAttachedToAsYouSource;
    private Player _you;

    private ChangeLife() {}

    public ChangeLife(DynParam<int> amount, bool forYou = false, bool forOpponent = false, bool forTargetPlayer = false,
      bool useAttachedToAsYouSource = false)
    {
      _amount = amount;

      _forYou = forYou;
      _forOpponent = forOpponent;
      _forTargetPlayer = forTargetPlayer;
      _useAttachedToAsYouSource = useAttachedToAsYouSource;

      RegisterDynamicParameters(amount);
    }

    protected override void Initialize()
    {
      _you = _useAttachedToAsYouSource
        ? Source.OwningCard.AttachedTo.Controller
        : Source.OwningCard.Controller;
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return _forTargetPlayer && Targets.Effect.Any(x => x == player) && _amount.Value < 0 ? Math.Abs(_amount.Value) : 0;
    }

    protected override void ResolveEffect()
    {
      if (_forYou)
      {
        _you.Life += _amount.Value;
      }

      if (_forOpponent)
      {
        _you.Opponent.Life += _amount.Value;
      }

      if (_forTargetPlayer)
      {
        Target.Player().Life += _amount;
      }
    }
  }
}