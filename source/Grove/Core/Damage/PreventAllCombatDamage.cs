namespace Grove
{
  using System;
  using Infrastructure;

  public class PreventAllCombatDamage : DamagePrevention
  {
    private readonly Func<Card, bool> _filter;
    private PreventAllCombatDamage() {}

    public PreventAllCombatDamage(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.IsCombat && _filter(p.Source))
        return p.Amount;

      return 0;
    }
  }
}