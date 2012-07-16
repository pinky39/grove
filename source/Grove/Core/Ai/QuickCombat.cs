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
      performance.LifepointsLeft = card => card.LifepointsLeft + toughnessIncrease;
      performance.DamageInSingleDamageStep = card => card.Power.Value + powerIncrease;

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
      evaluation.LeathalDamageHandler = ((_, amount) => leathalAmount += amount);
      var results = evaluation.Evaluate();                  
      var prevented = results.DamageDealt - attacker.LifepointsLeft;
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

      return attacker.TotalDamageThisCanDealInAllDamageSteps - blockers.Sum(x => x.LifepointsLeft);
    }

    public static int CalculateDefendingPlayerLifeloss(Card attacker, IEnumerable<Card> blockers)
    {
      if (blockers.None())
        return attacker.Power.Value;

      if (attacker.Has().Trample)
      {
        var totalToughness = blockers.Sum(x => x.Toughness);
        var diff = attacker.Power - totalToughness;

        return (diff > 0 ? diff : 0).Value;
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

    private static bool CanBlockerBeDealtLeathalCombatDamage(Card blocker, Card attacker, int additionalPower,
      int additionalThoughness)
    {
      var performance = new BlockerEvaluation(blocker, attacker);
      performance.DamageInSingleDamageStep = card => card.Power.Value + additionalPower;
      performance.LifepointsLeft = card => card.LifepointsLeft + additionalThoughness;

      return performance.Evaluate().ReceivesLeathalDamage;
    }
  }
}