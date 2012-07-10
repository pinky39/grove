namespace Grove.Core.Preventions
{
  using System;

  public class PreventDamageFromAnySource : DamagePrevention
  {
    public Func<DamagePrevention, int, int> Amount = delegate { return 0; };

    public override void PreventDamage(Damage damage)
    {      
      damage.Prevent(Amount(this, damage.Amount));
    }

    public void SetAmount(int amount)
    {
      Amount = delegate { return amount; };
    }

    public override int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat)
    {      
      var dealt = amount - Amount(this, amount);
      return dealt > 0 ? dealt : 0;
    }
  }
}