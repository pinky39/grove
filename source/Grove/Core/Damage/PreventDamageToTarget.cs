namespace Grove
{
  using System;
  using Infrastructure;

  public class PreventDamageToTarget : DamagePrevention
  {    
    private readonly Func<object, Context, int> _amount;
    private readonly object _target;
    private readonly Func<Card, Context, bool> _sourceSelector;

    private PreventDamageToTarget() {}

    public PreventDamageToTarget(
      object target, 
      Func<object, Context, int> amount = null, 
      Func<Card, Context, bool> sourceSelector = null)
    {
      _amount = amount ?? delegate { return int.MaxValue; };
      _sourceSelector = sourceSelector ?? delegate { return true; };
      _target = target;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        calc.Calculate(_target));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Target != _target)
        return 0;

      if (!_sourceSelector(p.Source, Ctx(p)))
        return 0;

      return _amount(_target, Ctx(p));                  
    }
  }
}