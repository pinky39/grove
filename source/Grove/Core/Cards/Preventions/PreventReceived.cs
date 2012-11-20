namespace Grove.Core.Cards.Preventions
{
  public class PreventReceived : DamagePrevention
  {
    public bool CombatOnly;

    public override int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      if (CombatOnly && !isCombat)
        return amount;

      return 0;
    }

    public override void PreventReceivedDamage(Damage damage)
    {
      damage.Amount = EvaluateReceivedDamage(damage.Source, damage.Amount, damage.IsCombat);
    }
  }
}