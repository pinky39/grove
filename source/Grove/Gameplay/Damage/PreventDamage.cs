namespace Grove.Gameplay.Damage
{
  using System;
  using Infrastructure;
  using Targeting;

  [Serializable]
  public class PreventDamage : DamagePrevention
  {
    private readonly int? _amount;
    private readonly Trackable<int> _amountLeft;
    private readonly Func<DamagePrevention, Card, bool> _sourceFilter;

    private PreventDamage() {}

    public PreventDamage(int? amount = null, Func<DamagePrevention, Card, bool> sourceFilter = null)
    {
      _amount = amount;

      if (amount.HasValue)
        _amountLeft = new Trackable<int>(_amount.Value);

      _sourceFilter = sourceFilter ?? delegate { return true; };
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

      var prevented = damage.Prevent(_amountLeft.Value);
      _amountLeft.Value -= prevented;
      return;
    }

    public override void Initialize(ITarget owner, Game game)
    {
      base.Initialize(owner, game);

      if (_amountLeft != null)
        _amountLeft.Initialize(game.ChangeTracker);
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      if (_amount == null)
        return 0;
      
      var dealt = amount - _amountLeft.Value;
      return dealt > 0 ? dealt : 0;
    }
  }
}