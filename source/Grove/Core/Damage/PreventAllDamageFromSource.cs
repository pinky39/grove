namespace Grove
{
  using Grove.Infrastructure;

  public class PreventAllDamageFromSource : DamagePrevention
  {
    private readonly Card _source;
    private readonly bool _preventCombatOnly;

    private PreventAllDamageFromSource() {}

    public PreventAllDamageFromSource(Card source, bool preventCombatOnly)
    {
      _source = source;
      _preventCombatOnly = preventCombatOnly;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        _preventCombatOnly.GetHashCode(),
        calc.Calculate(_source));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Source != _source)
        return 0;

      if (!p.IsCombat && _preventCombatOnly)
        return 0;

      return p.Amount;
    }
  }
}