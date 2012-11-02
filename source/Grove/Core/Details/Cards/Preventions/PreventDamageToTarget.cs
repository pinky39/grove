namespace Grove.Core.Details.Cards.Preventions
{
  using System;
  using Infrastructure;

  public class PreventDamageToTarget : DamagePrevention
  {
    private Trackable<int> _amountLeft;

    public int Amount;
    public bool AmountIsDepletable;
    
    public Func<DamagePrevention, Card, bool> SourceFilter = delegate { return true; };
    public bool PreventAll;

    public override void PreventReceivedDamage(Damage damage)
    {      
      
      
      if (!SourceFilter(this, damage.Source))
        return;
      
      if (PreventAll)
      {
        damage.PreventAll();
        return;
      }
      
      if (AmountIsDepletable)
      {
        var prevented = damage.Prevent(_amountLeft.Value);  
        _amountLeft.Value -= prevented;        
        return;
      }

      damage.Prevent(Amount);
    }

    protected override void Initialize()
    {      
      _amountLeft = new Trackable<int>(Amount, Game.ChangeTracker);
    }

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      var dealt = amount - _amountLeft.Value;
      return dealt > 0 ? dealt : 0;
    }
  }
}