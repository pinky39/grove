namespace Grove.Gameplay.DamageHandling
{
  public class PreventAllCombatDamage : DamagePrevention
  {
    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.IsCombat)
        return parameters.Amount;

      return 0;
    }
  }
}