namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;

  public class AttackerEvaluation
  {
    private readonly Card _attacker;
    private readonly List<Card> _blockers = new List<Card>();
    private readonly int _powerIncrease;
    private readonly int _toughnessIncrease;

    public Action<Card, int> BlockerHasDealtDamage = delegate { };
    public Action<Card, int> BlockerHasDealtLeathalDamage = delegate { };

    public AttackerEvaluation(Card attacker, IEnumerable<Card> blockers, int powerIncrease = 0, int toughnessIncrease = 0)
    {
      _attacker = attacker;
      _powerIncrease = powerIncrease;
      _toughnessIncrease = toughnessIncrease;
      _blockers.AddRange(blockers);
    }

    private int GetAttackerLifepoints()
    {
      return _attacker.Life + _toughnessIncrease;
    }

    public CalculationResults Evaluate()
    {
      var results = new CalculationResults();
      var blockers = _blockers.AsEnumerable();

      var maxDamageDealtByBlocker = 0;
      Card blockerThatDealsBiggestDamage = null;
      var total = 0;

      if (_attacker.Is().Creature == false)
        return results;

      if (_attacker.CanBeDestroyed == false)
        return results;

      if (_attacker.HasFirstStrike)
      {        
        blockers = RemoveBlockersThatWillBeKilledBeforeTheyDealDamage(blockers);
      }

      foreach (var blocker in blockers)
      {
        var amount = QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
          creature1: blocker, 
          creature2: _attacker);

        if (amount > 0 && blocker.Has().Deathtouch)
        {
          results.ReceivesLeathalDamage = true;
          BlockerHasDealtLeathalDamage(blocker, amount);
          results.LeathalBlocker = blocker;
        }

        BlockerHasDealtDamage(blocker, amount);
        total += amount;

        if (amount > maxDamageDealtByBlocker)
        {
          blockerThatDealsBiggestDamage = blocker;
          maxDamageDealtByBlocker = amount;
        }
      }

      results.DamageDealt = total;
      
      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage ||
        results.DamageDealt >= GetAttackerLifepoints();

      results.LeathalBlocker = results.LeathalBlocker ?? blockerThatDealsBiggestDamage;

      return results;
    }

    private IEnumerable<Card> RemoveBlockersThatWillBeKilledBeforeTheyDealDamage(IEnumerable<Card> blockers) {
      blockers = blockers.Where(blocker =>
        {
          var canBeKilledBeforeItDealsDamage = 
            blocker.HasFirstStrike || 
              blocker.Has().Indestructible ||
                blocker.Life > QuickCombat.GetAmountOfDamageCreature1WillDealToCreature2(
                  creature1: _attacker,  
                  creature2: blocker,
                  powerIncrease: _powerIncrease);

          return canBeKilledBeforeItDealsDamage;
        });
      return blockers;
    }

    public class CalculationResults
    {
      public int DamageDealt { get; set; }
      public bool ReceivesLeathalDamage { get; set; }
      public Card LeathalBlocker { get; set; }
    }
  }
}