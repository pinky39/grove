namespace Grove.Core.Preventions
{
  using System;
  using Grove.Infrastructure;

  public class PreventDamageToTarget : DamagePrevention
  {
    public int? Amount;    

    public Func<DamagePrevention, Card, bool> SourceFilter = delegate { return true; };
    private Trackable<int> _amountLeft;

    public override void PreventReceivedDamage(Damage damage)
    {
      if (!SourceFilter(this, damage.Source))
        return;

      if (Amount == null)
      {
        damage.PreventAll();
        return;
      }

      var prevented = damage.Prevent(_amountLeft.Value);
      _amountLeft.Value -= prevented;
      return;
    }

    protected override void Initialize()
    {
      if (Amount.HasValue)
        _amountLeft = new Trackable<int>(Amount.Value, Game.ChangeTracker);
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      var dealt = amount - _amountLeft.Value;
      return dealt > 0 ? dealt : 0;
    }
  }
}