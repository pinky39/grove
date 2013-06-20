namespace Grove.Gameplay.DamageHandling
{
  using System;
  using Infrastructure;

  public class PreventDamageToTarget : DamagePrevention
  {
    private readonly object _creatureOrPlayer;
    private readonly int _maxAmount;

    private readonly Func<Card, bool> _sourceFilter;

    private PreventDamageToTarget() {}

    public PreventDamageToTarget(object creatureOrPlayer, Func<Card, bool> sourceFilter = null, int maxAmount = int.MaxValue)
    {
      _creatureOrPlayer = creatureOrPlayer;
      _maxAmount = maxAmount;

      _sourceFilter = sourceFilter ?? delegate { return true; };
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
       GetType().GetHashCode(),
       _maxAmount,
       calc.Calculate((IHashable)_creatureOrPlayer));
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.Target != _creatureOrPlayer)
        return 0;

      if (!_sourceFilter(parameters.Source))
        return 0;

      return Math.Min(parameters.Amount, _maxAmount);
    }
  }
}