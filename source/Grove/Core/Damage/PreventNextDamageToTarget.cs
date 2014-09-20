namespace Grove
{
  using System;
  using Grove.Infrastructure;

  public class PreventNextDamageToTarget : DamagePrevention
  {
    private readonly Trackable<int> _amount;
    private readonly object _creatureOrPlayer;

    private PreventNextDamageToTarget() {}

    public PreventNextDamageToTarget(int amount, object creatureOrPlayer)
    {
      _amount = new Trackable<int>(amount);      
      _creatureOrPlayer = creatureOrPlayer;
    }

    public override int CalculateHash(HashCalculator calc)
    {
       return HashCalculator.Combine(
        GetType().GetHashCode(),
        _amount.Value,
        calc.Calculate((IHashable)_creatureOrPlayer));
    }

    protected override void Initialize()
    {
      _amount.Initialize(ChangeTracker);
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Target != _creatureOrPlayer)
        return 0;

      var prevented = Math.Min(_amount, p.Amount);

      if (!p.QueryOnly)
        _amount.Value -= prevented;

      return prevented;
    }
  }
}