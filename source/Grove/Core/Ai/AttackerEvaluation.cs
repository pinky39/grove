namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class AttackerEvaluation
  {
    private readonly Card _attacker;
    private readonly List<Card> _blockers = new List<Card>();

    public Func<Card, int> CalculateCombatDamage = card => card.CalculateCombatDamage();
    public Action<Card, int> BlockerHasDealtDamage = delegate { };
    public Action<Card, int> BlockerHasDealtLeathalDamage = delegate { };
    public Func<Card, int> CalculateLifepointsLeft = card => card.Life;

    public AttackerEvaluation(Card attacker, IEnumerable<Card> blockers)
    {
      _attacker = attacker;
      _blockers.AddRange(blockers);      
    }    

    public CalculationResults Evaluate()
    {
      var results = new CalculationResults();
      var blockers = _blockers.AsEnumerable();
      
      var biggestDamage = 0;
      Card blockerThatDealsBiggestDamage = null;
      var total = 0;

      if (_attacker.Is().Creature == false)
        return results;

      if (_attacker.CanBeDestroyed == false)
        return results;

      if (_attacker.HasFirstStrike)
      {
        // eliminate blockers that will be killed
        // before they can deal damage

        blockers = blockers.Where(
          b => (b.HasFirstStrike || b.Has().Indestructible || b.Life > CalculateCombatDamage(_attacker)));
      }

      foreach (var blocker in blockers)
      {
        var dealtAmount = _attacker.EvaluateReceivedDamage(blocker, blocker.CalculateCombatDamage(), isCombat: true);

        if (dealtAmount > 0 && blocker.Has().Deathtouch)
        {
          results.ReceivesLeathalDamage = true;
          BlockerHasDealtLeathalDamage(blocker, dealtAmount);
          results.LeathalBlocker = blocker;
        }

        BlockerHasDealtDamage(blocker, dealtAmount);
        total += dealtAmount;
        
        if (dealtAmount > biggestDamage)
        {
          blockerThatDealsBiggestDamage = blocker;
        }
      }

      results.DamageDealt = total;
      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage || results.DamageDealt >= CalculateLifepointsLeft(_attacker);
      results.LeathalBlocker = results.LeathalBlocker ?? blockerThatDealsBiggestDamage;

      return results;
    }

    public class CalculationResults
    {
      public int  DamageDealt { get; set; }
      public bool ReceivesLeathalDamage { get; set; }
      public Card LeathalBlocker { get; set; }      
  }
}
}