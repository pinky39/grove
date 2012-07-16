namespace Grove.Core.Ai
{
  using System;

  public class BlockerEvaluation
  {
    private readonly Card _blocker;
    private readonly Card _attacker;

    public BlockerEvaluation(Card blocker, Card attacker)
    {
      _blocker = blocker;
      _attacker = attacker;            
    }

    public Func<Card, int> DamageInSingleDamageStep = card => card.Power.Value;
    public Func<Card, int> LifepointsLeft = card => card.LifepointsLeft;    

    public CalculationResults Evaluate()
    {
      var results = new CalculationResults();
      
      if (_blocker.Is().Creature == false)
        return results;

      if (_blocker.CanBeDestroyed == false)
        return results;

      if (_blocker.HasFirstStrike && !_attacker.HasFirstStrike && !_attacker.Has().Indestructible)
      {
        var blockerDealtAmount = _attacker.EvaluateHowMuchDamageCanBeDealt(
          _blocker, DamageInSingleDamageStep(_blocker), isCombat: true);

        if (blockerDealtAmount > 0 && _blocker.Has().Deathtouch)
        {
          return results;
        }

        if (blockerDealtAmount >= _attacker.LifepointsLeft)
          return results;
      }

      var attackerDealtAmount = _blocker.EvaluateHowMuchDamageCanBeDealt(_attacker, 
        _attacker.Power.Value, isCombat: true);

      if (attackerDealtAmount == 0)
        return results;

      if (_attacker.Has().Deathtouch)
      {
        results.ReceivesLeathalDamage = true;
      }

      results.DamageDealt = attackerDealtAmount;
      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage || attackerDealtAmount >= LifepointsLeft(_blocker);

      return results;
    }

    public class CalculationResults
    {
      public int DamageDealt { get; set; }
      public bool ReceivesLeathalDamage { get; set; }      
    }
  }
}