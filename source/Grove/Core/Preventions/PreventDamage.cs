namespace Grove.Core.Preventions
{
  using System;
  using Infrastructure;
  using Targeting;

  public class PreventDamage : DamagePrevention
  {
    private readonly int? _amount;
    private readonly Trackable<int?> _amountLeft = new Trackable<int?>();
    private readonly Func<DamagePrevention, Card, bool> _sourceFilter;

    private PreventDamage() {}

    public PreventDamage(int? amount = null, Func<DamagePrevention, Card, bool> sourceFilter = null)
    {
      _amount = amount;
      _sourceFilter = sourceFilter;
    }

    public override void PreventReceivedDamage(Damage damage)
    {
      if (!_sourceFilter(this, damage.Source))
        return;

      if (_amount == null)
      {
        damage.PreventAll();
        return;
      }

      var prevented = damage.Prevent(_amountLeft.Value.GetValueOrDefault());
      _amountLeft.Value -= prevented;
      return;
    }

    public override void Initialize(ITarget owner, Game game)
    {
      base.Initialize(owner, game);
      _amountLeft.Initialize(game.ChangeTracker);
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      var dealt = amount - _amountLeft.Value;
      return dealt > 0 ? dealt.GetValueOrDefault() : 0;
    }
  }
}