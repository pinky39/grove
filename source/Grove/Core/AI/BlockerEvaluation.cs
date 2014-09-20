namespace Grove.AI
{
  public class BlockerEvaluation
  {
    private readonly BlockerEvaluationParameters _p;

    public BlockerEvaluation(BlockerEvaluationParameters p)
    {
      _p = p;
    }

    private int GetBlockerLifepoints()
    {
      return _p.Blocker.Life + _p.BlockerToughnessIncrease;
    }

    private int GetAttackerLifepoints()
    {
      return _p.Attacker.Life + _p.AttackerToughnessIncrease;
    }

    public Results Evaluate()
    {
      var results = new Results
        {
          DamageDealt = 0,
          ReceivesLeathalDamage = false
        };

      if (_p.Attacker == null)
      {
        return results;
      }

      if (_p.Blocker.Is().Creature == false)
        return results;

      if (_p.Blocker.CanBeDestroyed == false)
        return results;


      if (_p.Blocker.HasFirstStrike && !_p.Attacker.HasFirstStrike && !_p.Attacker.Has().Indestructible)
      {
        var blockerDealtAmount = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
          creature1: _p.Blocker,
          creature2: _p.Attacker,
          powerIncrease: _p.BlockerPowerIncrease);

        if (blockerDealtAmount > 0 && _p.Blocker.Has().Deathtouch)
        {
          return results;
        }

        if (blockerDealtAmount >= GetAttackerLifepoints())
          return results;
      }

      var attackerDealtAmount = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
        creature1: _p.Attacker,
        creature2: _p.Blocker,
        powerIncrease: _p.AttackerPowerIncrease);

      if (attackerDealtAmount == 0)
        return results;


      if (_p.Attacker.Has().Deathtouch)
      {
        results.ReceivesLeathalDamage = true;
      }

      results.DamageDealt = attackerDealtAmount;
      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage || attackerDealtAmount >= GetBlockerLifepoints();

      return results;
    }

    public class Results
    {
      public int DamageDealt;
      public bool ReceivesLeathalDamage;
    }
  }
}