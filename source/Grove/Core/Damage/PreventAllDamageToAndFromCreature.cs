namespace Grove
{
  using Grove.Infrastructure;

  public class PreventAllDamageToAndFromCreature : DamagePrevention
  {
    private readonly Card _creature;
    private readonly bool _combatOnly;

    private PreventAllDamageToAndFromCreature() {}

    public PreventAllDamageToAndFromCreature(Card creature, bool combatOnly)
    {
      _creature = creature;
      _combatOnly = combatOnly;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(_creature));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (_combatOnly && !p.IsCombat)
        return 0;
      
      if (p.Source == Modifier.SourceCard)
        return p.Amount;

      if (p.Target == Modifier.SourceCard)
        return p.Amount;

      return 0;
    }
  }
}