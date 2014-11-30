namespace Grove
{
  using System;
  using Grove.Infrastructure;

  public class PreventDamageToTarget : DamagePrevention
  {
    private readonly object _target;
    private readonly int _maxAmount;

    private readonly Func<Card, bool> _sourceFilter;

    private PreventDamageToTarget() {}

    public PreventDamageToTarget(object target, Func<Card, bool> sourceFilter = null, int maxAmount = int.MaxValue)
    {
      _target = target;
      _maxAmount = maxAmount;

      _sourceFilter = sourceFilter ?? delegate { return true; };
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
       GetType().GetHashCode(),
       _maxAmount,
       calc.Calculate((IHashable)_target));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Target != _target)
        return 0;

      if (!_sourceFilter(p.Source))
        return 0;

      return Math.Min(p.Amount, _maxAmount);
    }
  }
}