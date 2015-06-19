namespace Grove
{
  using System;
  using Infrastructure;

  public class PreventDamageToCreatureOrPlayer : DamagePrevention
  {    
    private readonly Func<object, Context, int> _amount;
    private readonly object _creatureOrPlayer;
    private readonly Func<Card, Context, bool> _sourceRestriction;

    private PreventDamageToCreatureOrPlayer() {}

    public PreventDamageToCreatureOrPlayer(
      object creatureOrPlayer, 
      Func<object, Context, int> amount = null, 
      Func<Card, Context, bool> sourceRestriction = null)
    {
      _amount = amount ?? delegate { return int.MaxValue; };
      _sourceRestriction = sourceRestriction ?? delegate { return true; };
      _creatureOrPlayer = creatureOrPlayer;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        calc.Calculate(_creatureOrPlayer));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Target != _creatureOrPlayer)
        return 0;

      if (!_sourceRestriction(p.Source, Ctx(p)))
        return 0;

      return _amount(_creatureOrPlayer, Ctx(p));                  
    }
  }
}