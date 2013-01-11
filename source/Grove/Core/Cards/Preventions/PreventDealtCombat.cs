namespace Grove.Core.Cards.Preventions
{
  public class PreventDealtCombat : DamagePrevention
  {        
    public override int PreventDealtCombatDamage(int amount)
    {
      return 0;
    }
  }
}