namespace Grove.Gameplay.DamageHandling
{
  using Infrastructure;

  public class PreventAllDamageToAndFromCreature : DamagePrevention
  {
    private readonly Card _creature;

    private PreventAllDamageToAndFromCreature() {}

    public PreventAllDamageToAndFromCreature(Card creature)
    {
      _creature = creature;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(_creature));
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.Source == Modifier.SourceCard)
        return parameters.Amount;

      if (parameters.Target == Modifier.SourceCard)
        return parameters.Amount;

      return 0;
    }
  }
}