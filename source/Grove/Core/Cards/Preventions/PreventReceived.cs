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
  }
}