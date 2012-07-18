namespace Grove.Core.Details.Cards.Preventions
{
  using Infrastructure;

  public class PreventDamageToTarget : DamagePrevention
  {
    private Trackable<int> _amountLeft;

    public int Amount;
    public bool AmountIsDepletable;

    public override void PreventReceivedDamage(Damage damage)
    {      
      var prevented = damage.Prevent(_amountLeft.Value);

      if (AmountIsDepletable)
      {        
        _amountLeft.Value -= prevented;        
      }
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