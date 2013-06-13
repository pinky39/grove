namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Infrastructure;

  public class QuickCombat
  {
    public static bool CanAttackerBeDealtLeathalDamage(AttackerEvaluationParameters p)
    {
      var attackerEvaluation = new AttackerEvaluation(p);
      var results = attackerEvaluation.Evaluate();
      return results.ReceivesLeathalDamage;
    }

    public static bool CanAttackerBeDealtLeathalDamage(Card attacker, IEnumerable<Card> blockers)
    {
      return CanAttackerBeDealtLeathalDamage(new AttackerEvaluationParameters(attacker, blockers));
    }

    public static Card GetBlockerThatDealsLeathalDamageToAttacker(Card attacker, IEnumerable<Card> blockers)
    {
      var p = new AttackerEvaluationParameters(attacker, blockers);

      var performance = new AttackerEvaluation(p);
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
      var evaluation = new BlockerEvaluation(new BlockerEvaluationParameters {Blocker = blocker, Attacker = attacker});
      var results = evaluation.Evaluate();

      if (results.ReceivesLeathalDamage)
        return results.DamageDealt;

      return 0;
    }

    public static int GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(Card attacker,
      IEnumerable<Card> blockers)
    {
      var leathalAmount = 0;
      var p = new AttackerEvaluationParameters(attacker, blockers);
      var evaluation = new AttackerEvaluation(p);
      evaluation.BlockerHasDealtLeathalDamage = ((_, amount) => leathalAmount += amount);
      var results = evaluation.Evaluate();
      var prevented = results.DamageDealt - attacker.Life;
      return Math.Max(leathalAmount, prevented);
    }

    public static int GetAmountOfDamageThatWillBeDealtToAttacker(AttackerEvaluationParameters p)
    {
      var attackerEvaluation = new AttackerEvaluation(p);
      var results = attackerEvaluation.Evaluate();
      return results.DamageDealt;
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

      var p = new AttackerEvaluationParameters(attacker, blockers);

      var canBeDealtLeathalDamageWithoutBoost = CanAttackerBeDealtLeathalDamage(p);

      if (canBeDealtLeathalDamageWithoutBoost == false)
        return 0;

      p.AttackerPowerIncrease = powerIncrease;
      p.AttackerToughnessIncrease = toughnessIncrease;

      var canBeDealtLeathalDamageWithBoost = CanAttackerBeDealtLeathalDamage(p);

      return canBeDealtLeathalDamageWithBoost ? 0 : attacker.Score;
    }

    public static int CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(Card attacker,
      Card blocker, int powerIncrease, int toughnessIncrease)
    {
      if (attacker == null)
        return 0;

      var p = new BlockerEvaluationParameters
        {
          Attacker = attacker,
          Blocker = blocker,
        };

      var canBeDealtLeathalDamageWithoutBoost = CanBlockerBeDealtLeathalCombatDamage(p);

      if (canBeDealtLeathalDamageWithoutBoost == false)
        return 0;

      p.BlockerPowerIncrease += powerIncrease;
      p.BlockerToughnessIncrease += toughnessIncrease;

      var canBeDealtLeathalDamageWithBoost = CanBlockerBeDealtLeathalCombatDamage(p);
      return canBeDealtLeathalDamageWithBoost == false ? blocker.Score : 1;
    }

    public static int CalculateTrampleDamage(Card attacker, Card blocker)
    {
      return CalculateTrampleDamage(attacker, blocker.ToEnumerable());
    }

    public static bool CanBlockerBeDealtLeathalCombatDamage(Card attacker, Card blocker)
    {
      return
        CanBlockerBeDealtLeathalCombatDamage(new BlockerEvaluationParameters {Attacker = attacker, Blocker = blocker});
    }

    public static bool CanBlockerBeDealtLeathalCombatDamage(BlockerEvaluationParameters p)
    {
      var blockerEvaluation = new BlockerEvaluation(p);
      var results = blockerEvaluation.Evaluate();

      return results.ReceivesLeathalDamage;
    }

    public static bool CanAttackerKillAnyBlocker(Card attacker, IEnumerable<Card> blockers)
    {
      return blockers.Any(blocker => CanBlockerBeDealtLeathalCombatDamage(attacker, blocker));
    }
  }
}