namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class QuickCombat
  {
    private static bool CanAttackerBeDealtLeathalDamage(Card attacker, IEnumerable<Card> blockers, int powerIncrease,
      int toughnessIncrease)
    {
      var performance = new AttackerEvaluation(attacker, blockers);
      performance.CalculateLifepointsLeft = card => card.CalculateLifepointsLeft() + toughnessIncrease;
      performance.CalculateCombatDamage = card => card.CalculateCombatDamage(powerIncrease: powerIncrease);

      var results = performance.Evaluate();
      return results.ReceivesLeathalDamage;
    }

    public static Card GetBlockerThatDealsLeathalDamageToAttacker(Card attacker, IEnumerable<Card> blockers)
    {
      var performance = new AttackerEvaluation(attacker, blockers);
      var results = performance.Evaluate();

      return results.LeathalBlocker;
    }

    public static int GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(Card blocker, Card attacker)
    {
      var evaluation = new BlockerEvaluation(blocker, attacker);
      var results = evaluation.Evaluate();

      if (results.ReceivesLeathalDamage)
        return results.DamageDealt;

      return 0;
    }

    public static int GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(Card attacker, IEnumerable<Card> blockers)
    {
      var leathalAmount = 0;
      var evaluation = new AttackerEvaluation(attacker, blockers);
      evaluation.BlockerHasDealtLeathalDamage = ((_, amount) => leathalAmount += amount);
      var results = evaluation.Evaluate();                  
      var prevented = results.DamageDealt - attacker.CalculateLifepointsLeft();
      return Math.Max(leathalAmount, prevented);                        
    }

    public static Card GetAttackerThatDealsLeathalDamageToBlocker(Card blocker, Card attacker)
    {
      if (CanBlockerBeDealtLeathalCombatDamage(blocker, attacker))
      {
        return attacker;
      }
      return blocker;
    }

    public static bool CanAttackerBeDealtLeathalDamage(Card attacker, IEnumerable<Card> blockers)
    {
      var results = new AttackerEvaluation(attacker, blockers)
        .Evaluate();

      return results.ReceivesLeathalDamage;
    }
    
    public static int GetAmountOfDamageThatWillBeDealtToAttacker(Card attacker, IEnumerable<Card> blockers)
    {
      var performance = new AttackerEvaluation(attacker, blockers);      
      return performance.Evaluate().DamageDealt;
    }

    public static int CalculateTrampleDamage(Card attacker, IEnumerable<Card> blockers)
    {
      if (attacker.Has().Trample == false)
        return 0;

      return attacker.CalculateCombatDamage(allDamageSteps: true) - blockers.Sum(x => x.CalculateLifepointsLeft());
    }

    public static int CalculateDefendingPlayerLifeloss(Card attacker, IEnumerable<Card> blockers)
    {
      if (blockers.None())
        return attacker.CalculateCombatDamage(allDamageSteps: true);

      if (attacker.Has().Trample)
      {
        var totalLifepoints = blockers.Sum(x => x.CalculateLifepointsLeft());
        var diff = attacker.CalculateCombatDamage(allDamageSteps: true) - totalLifepoints;

        return diff > 0 ? diff : 0;
      }

      return 0;
    }

    public static int CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(Card attacker,
      IEnumerable<Card> blockers, int powerIncrease, int toughnessIncrease)
    {
      if (blockers.None() && powerIncrease > 0)
      {
        return 2;
      }

      if (toughnessIncrease < 1)
        return 0;

      var canBeDealtLeathalDamageWithoutBoost = CanAttackerBeDealtLeathalDamage(attacker, blockers);

      if (canBeDealtLeathalDamageWithoutBoost == false)
        return 0;

      var canBeDealtLeathalDamageWithBoost = CanAttackerBeDealtLeathalDamage(attacker, blockers, powerIncrease,
        toughnessIncrease);
      return canBeDealtLeathalDamageWithBoost == false ? attacker.Score : 1;
    }

    public static int CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(Card attacker,
      Card blocker, int powerIncrease, int toughnessIncrease)
    {
      if (attacker == null)
        return 0;

      var canBeDealtLeathalDamageWithoutBoost = CanBlockerBeDealtLeathalCombatDamage(blocker, attacker);

      if (canBeDealtLeathalDamageWithoutBoost == false)
        return 0;

      var canBeDealtLeathalDamageWithBoost = CanBlockerBeDealtLeathalCombatDamage(blocker, attacker, powerIncrease,
        toughnessIncrease);
      return canBeDealtLeathalDamageWithBoost == false ? blocker.Score : 1;
    }

    public static int CalculateTrampleDamage(Card attacker, Card blocker)
    {
      return CalculateTrampleDamage(attacker, blocker.ToEnumerable());
    }

    public static bool CanBlockerBeDealtLeathalCombatDamage(Card blocker, Card attacker)
    {
      var performance = new BlockerEvaluation(blocker, attacker);
      var results = performance.Evaluate();

      return results.ReceivesLeathalDamage;
    }

    private static bool CanBlockerBeDealtLeathalCombatDamage(Card blocker, Card attacker, int powerIncrease,
      int additionalThoughness)
    {
      var performance = new BlockerEvaluation(blocker, attacker);
      performance.CalculateCombatDamage = card => card.CalculateCombatDamage(powerIncrease: powerIncrease);
      performance.LifepointsLeft = card => card.CalculateLifepointsLeft() + additionalThoughness;

      return performance.Evaluate().ReceivesLeathalDamage;
    }

    public static bool CanAttackerKillAnyBlocker(Card attacker, IEnumerable<Card> blockers)
    {
      foreach (var blocker in blockers)
      {
        var eval = new BlockerEvaluation(blocker, attacker)
          .Evaluate();  

        if (eval.ReceivesLeathalDamage)
          return true;
      }

      return false;
    }    
  }
}