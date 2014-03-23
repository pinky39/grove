namespace Grove
{
  using Infrastructure;

  public class PreventAllCombatDamage : DamagePrevention
  {
    public override int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.IsCombat)
        return p.Amount;

      return 0;
    }
  }
}