namespace Grove.Artifical
{
  using Gameplay;

  public class BlockerEvaluation
  {
    private readonly Card _attacker;
    private readonly Card _blocker;
    private readonly int _powerIncrease;
    private readonly int _toughnessIncrease;

    public BlockerEvaluation(Card blocker, Card attacker, int powerIncrease = 0, int toughnessIncrease = 0)
    {
      _blocker = blocker;
      _attacker = attacker;
      _powerIncrease = powerIncrease;
      _toughnessIncrease = toughnessIncrease;
    }

    private int GetBlockerLifepoints()
    {
      return _blocker.Life + _toughnessIncrease;
    }

    public CalculationResults Evaluate()
    {
      var results = new CalculationResults();

      if (_attacker == null)
      {
        return results;
      }

      if (_blocker.Is().Creature == false)
        return results;

      if (_blocker.CanBeDestroyed == false)
        return results;

      if (_blocker.HasFirstStrike && !_attacker.HasFirstStrike && !_attacker.Has().Indestructible)
      {
        var blockerDealtAmount = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
          creature1: _blocker,
          creature2: _attacker,
          powerIncrease: _powerIncrease);

        if (blockerDealtAmount > 0 && _blocker.Has().Deathtouch)
        {
          return results;
        }

        if (blockerDealtAmount >= _attacker.Life)
          return results;
      }

      var attackerDealtAmount = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
        creature1: _attacker,
        creature2: _blocker);


      if (attackerDealtAmount == 0)
        return results;

      if (_attacker.Has().Deathtouch)
      {
        results.ReceivesLeathalDamage = true;
      }

      results.DamageDealt = attackerDealtAmount;
      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage || attackerDealtAmount >= GetBlockerLifepoints();

      return results;
    }

    public class CalculationResults
    {
      public int DamageDealt { get; set; }
      public bool ReceivesLeathalDamage { get; set; }
    }
  }
}