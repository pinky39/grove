namespace Grove.Gameplay.DamageHandling
{
  using Infrastructure;

  public class PreventAllCombatDamage : DamagePrevention
  {
    public override int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.IsCombat)
        return parameters.Amount;

      return 0;
    }
  }
}