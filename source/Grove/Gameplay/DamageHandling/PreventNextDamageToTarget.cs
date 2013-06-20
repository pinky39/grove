namespace Grove.Gameplay.DamageHandling
{
  using System;
  using Infrastructure;

  public class PreventNextDamageToTarget : DamagePrevention
  {
    private readonly Trackable<int> _amount;
    private readonly object _creatureOrPlayer;

    private PreventNextDamageToTarget() {}

    public PreventNextDamageToTarget(int amount, object creatureOrPlayer)
    {
      _amount = new Trackable<int>(amount);
      ;
      _creatureOrPlayer = creatureOrPlayer;
    }

    public override int CalculateHash(HashCalculator calc)
    {
       return HashCalculator.Combine(
        GetType().GetHashCode(),
        _amount.Value,
        calc.Calculate((IHashable)_creatureOrPlayer));
    }

    public override int PreventDamage(PreventDamageParameters parameters)
    {
      if (parameters.Target != _creatureOrPlayer)
        return 0;

      var prevented = Math.Min(_amount, parameters.Amount);

      if (!parameters.QueryOnly)
        _amount.Value -= prevented;

      return prevented;
    }
  }
}