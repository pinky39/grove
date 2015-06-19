namespace Grove
{
  using Infrastructure;

  public class PreventAllDamageDealtToAndByCreature : DamagePrevention
  {
    private readonly Card _creature;
    private readonly bool _combatOnly;

    private PreventAllDamageDealtToAndByCreature() {}

    public PreventAllDamageDealtToAndByCreature(Card creature, bool combatOnly)
    {
      _creature = creature;
      _combatOnly = combatOnly;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
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