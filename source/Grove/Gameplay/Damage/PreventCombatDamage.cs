namespace Grove.Core.Preventions
{
  public class PreventCombatDamage : DamagePrevention
  {        
    public override int PreventDealtCombatDamage(int amount)
    {
      return 0;
    }
  }
}