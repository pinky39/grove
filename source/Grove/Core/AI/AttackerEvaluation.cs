namespace Grove.AI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class AttackerEvaluation
  {
    private readonly AttackerEvaluationParameters _p;
    
    public AttackerEvaluation(AttackerEvaluationParameters p)
    {
      _p = p;
    }

    public Results Evaluate()
    {
      var results = new Results();

      if (!_p.Attacker.Is().Creature || !_p.Attacker.CanBeDestroyed)
        return results;

      var blockers = _p.Blockers;
      if (_p.Attacker.HasFirstStrike)
      {
        blockers = GetOnlyBlockersThatWontBeKilledBeforeTheyDealDamage().ToList();
      }
      
      var maxDamageDealtByAnyBlocker = 0;
      Card blockerThatDealsMaxDamage = null;

      foreach (var blocker in blockers)
      {
        var amount = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
          creature1: blocker.Card,
          creature2: _p.Attacker,
          powerIncrease: blocker.PowerIncrease);

        if (amount > 0 && blocker.Card.Has().Deathtouch)
        {
          results.ReceivesLeathalDamage = true;
          results.DeathTouchDamage += amount;          
          results.LeathalBlocker = blocker.Card;
        }
        
        results.TotalDamage += amount;

        if (amount > maxDamageDealtByAnyBlocker)
        {
          blockerThatDealsMaxDamage = blocker.Card;
          maxDamageDealtByAnyBlocker = amount;
        }
      }      

      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage ||
        results.TotalDamage >= _p.Attacker.Life + _p.AttackerToughnessIncrease;

      results.LeathalBlocker = results.LeathalBlocker ?? blockerThatDealsMaxDamage;
      return results;
    }

    private IEnumerable<AttackerEvaluationParameters.Blocker> GetOnlyBlockersThatWontBeKilledBeforeTheyDealDamage()
    {
      return _p.Blockers.Where(blocker =>
        {
          if (blocker.Card.HasFirstStrike || blocker.Card.Has().Indestructible)
            return true;

          var attackersDamage = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
            creature1: _p.Attacker,
            creature2: blocker.Card,
            powerIncrease: _p.AttackerPowerIncrease);

          return blocker.Card.Life + blocker.ToughnessIncrease > attackersDamage;
        });
    }

    public class Results
    {      
      public Card LeathalBlocker;
      public bool ReceivesLeathalDamage;
      public int TotalDamage;
      public int DeathTouchDamage;
    }
  }
}