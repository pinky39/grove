namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;

  public class QuickCombat
  {
    private static bool CanAttackerBeDealtLeathalDamage(Card attacker, IEnumerable<Card> blockers, int powerIncrease,
      int toughnessIncrease)
    {
      var attackerEvaluation = new AttackerEvaluation(attacker, blockers, powerIncrease, toughnessIncrease);
      var results = attackerEvaluation.Evaluate();
      return results.ReceivesLeathalDamage;
    }

    public static Card GetBlockerThatDealsLeathalDamageToAttacker(Card attacker, IEnumerable<Card> blockers)
    {
      var performance = new AttackerEvaluation(attacker, blockers);
      var results = performance.Evaluate();

      return results.LeathalBlocker;
    }

    public static int GetAmountOfDamageCreature1WillDealToCreature2(Card creature1, Card creature2,
      int powerIncrease = 0)
    {
      var amountDealt = creature1.CalculateCombatDamageAmount(powerIncrease: powerIncrease);
      var preventedReceived = creature2.CalculatePreventedDamageAmount(amountDealt, creature1, isCombat: true);

      return amountDealt - preventedReceived;
    }

    public static int GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(Card blocker, Card attacker)
    {
      var evaluation = new BlockerEvaluation(blocker, attacker);
      var results = evaluation.Evaluate();

      if (results.ReceivesLeathalDamage)
        return results.DamageDealt;

      return 0;
    }

    public static int GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(Card attacker,
      IEnumerable<Card> blockers)
    {
      var leathalAmount = 0;
      var evaluation = new AttackerEvaluation(attacker, blockers);
      evaluation.BlockerHasDealtLeathalDamage = ((_, amount) => leathalAmount += amount);
      var results = evaluation.Evaluate();
      var prevented = results.DamageDealt - attacker.Life;
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

      var total = attacker.CalculateCombatDamageAmount(singleDamageStep: false);
      return total - blockers.Sum(x => x.Life);
    }

    public static int CalculateDefendingPlayerLifeloss(Card attacker, IEnumerable<Card> blockers)
    {
      var total = 0;

      if (blockers.None())
      {
        total = attacker.CalculateCombatDamageAmount(singleDamageStep: false);
      }
      else if (attacker.Has().Trample)
      {
        total = CalculateTrampleDamage(attacker, blockers);
      }

      var prevented = attacker.Controller.Opponent.CalculatePreventedReceivedDamageAmount(total, attacker,
        isCombat: true);
      return total - prevented;
    }

    public static int CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(Card attacker,
      IEnumerable<Card> blockers, int powerIncrease, int toughnessIncrease)
    {
      if ((blockers.None() || attacker.Has().Trample) && powerIncrease > 0)
      {
        return CalculateDefendingPlayerLifeloss(attacker, blockers) > 0 ? 2 : 0;
      }

      if (toughnessIncrease < 1)
        return 0;

      var canBeDealtLeathalDamageWithoutBoost = CanAttackerBeDealtLeathalDamage(attacker, blockers);

      if (canBeDealtLeathalDamageWithoutBoost == false)
        return 0;

      var canBeDealtLeathalDamageWithBoost = CanAttackerBeDealtLeathalDamage(attacker, blockers, powerIncrease,
        toughnessIncrease);

      return canBeDealtLeathalDamageWithBoost ? 0 : attacker.Score;
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
      var blockerEvaluation = new BlockerEvaluation(blocker, attacker);
      var results = blockerEvaluation.Evaluate();

      return results.ReceivesLeathalDamage;
    }

    private static bool CanBlockerBeDealtLeathalCombatDamage(Card blocker, Card attacker, int powerIncrease,
      int toughnessIncrease)
    {
      var evaluation = new BlockerEvaluation(blocker, attacker, powerIncrease, toughnessIncrease);
      return evaluation.Evaluate().ReceivesLeathalDamage;
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