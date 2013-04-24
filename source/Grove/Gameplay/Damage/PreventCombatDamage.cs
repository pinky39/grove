namespace Grove.Gameplay.Damage
{
  public class PreventCombatDamage : DamagePrevention
  {        
    public override int PreventDealtCombatDamage(int amount)
    {
      return 0;
    }
  }
}