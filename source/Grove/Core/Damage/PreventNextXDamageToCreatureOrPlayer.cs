namespace Grove
{
  using System;
  using Infrastructure;

  public class PreventNextXDamageToCreatureOrPlayer : DamagePrevention
  {
    private readonly Trackable<int> _amountLeft;
    private readonly object _creatureOrPlayer;

    private PreventNextXDamageToCreatureOrPlayer() {}

    public PreventNextXDamageToCreatureOrPlayer(int amount, object creatureOrPlayer)
    {
      _amountLeft = new Trackable<int>(amount);
      _creatureOrPlayer = creatureOrPlayer;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        _amountLeft.Value);
    }

    protected override void Initialize()
    {
      _amountLeft.Initialize(ChangeTracker);
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (p.Target != _creatureOrPlayer)
        return 0;

      var prevented = Math.Min(_amountLeft, p.Amount);

      if (!p.QueryOnly)
        _amountLeft.Value -= prevented;

      return prevented;
    }
  }
}