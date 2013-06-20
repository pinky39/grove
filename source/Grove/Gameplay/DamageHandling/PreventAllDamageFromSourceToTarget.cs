namespace Grove.Gameplay.DamageHandling
{
  using Infrastructure;

  public class PreventAllDamageFromSourceToTarget : DamagePrevention
  {
    private readonly object _creatureOrPlayer;
    private readonly bool _onlyOnce;
    private readonly Card _source;

    private PreventAllDamageFromSourceToTarget() {}

    public PreventAllDamageFromSourceToTarget(Card source, object creatureOrPlayer, bool onlyOnce)
    {
      _onlyOnce = onlyOnce;
      _source = source;
      _creatureOrPlayer = creatureOrPlayer;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        GetType().GetHashCode(),
        calc.Calculate(_source),
        calc.Calculate((IHashable) _creatureOrPlayer));
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.Source == _source && parameters.Target == _creatureOrPlayer)
      {
        if (_onlyOnce && !parameters.QueryOnly)
          EndOfLife.Raise();

        return parameters.Amount;
      }

      return 0;
    }
  }
}