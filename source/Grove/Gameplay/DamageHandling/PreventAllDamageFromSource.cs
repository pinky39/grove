namespace Grove.Gameplay.DamageHandling
{
  using Infrastructure;

  public class PreventAllDamageFromSource : DamagePrevention
  {
    private readonly Card _source;

    private PreventAllDamageFromSource() {}

    public PreventAllDamageFromSource(Card source)
    {
      _source = source;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(_source));
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.Source != _source)
        return 0;

      return parameters.Amount;
    }
  }
}