namespace Grove.Core.Details.Cards.Preventions
{
  public class PreventDamageFromAnySource : DamagePrevention
  {
    public int Amount;

    public override void PreventDamage(Damage damage)
    {
      damage.Prevent(Amount);
    }

    public override int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat)
    {
      var dealt = amount - Amount;
      return dealt > 0 ? dealt : 0;
    }
  }
}