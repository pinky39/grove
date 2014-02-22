namespace Grove.Gameplay
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

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (_combatOnly && !parameters.IsCombat)
        return 0;
      
      if (parameters.Source == Modifier.SourceCard)
        return parameters.Amount;

      if (parameters.Target == Modifier.SourceCard)
        return parameters.Amount;

      return 0;
    }
  }
}