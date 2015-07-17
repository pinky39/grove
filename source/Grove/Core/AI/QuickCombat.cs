namespace Grove.AI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;

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
      var p = new AttackerEvaluationParameters(attacker, blockers);
      var results = new AttackerEvaluation(p).Evaluate();
      
      if (!results.ReceivesLeathalDamage)
        return 0;

      if (results.DeathTouchDamage > 0)
        return results.DeathTouchDamage;
      
      var prevented = results.TotalDamage - attacker.Life + 1;
      return prevented;
    }

    public static int GetAmountOfDamageThatWillBeDealtToAttacker(AttackerEvaluationParameters p)
    {
      var results = new AttackerEvaluation(p).Evaluate();
      return results.TotalDamage;
    }

    public static int CalculateTrampleDamage(Card attacker, IEnumerable<Card> blockers)
    {
      var p = new AttackerEvaluationParameters(attacker, blockers);
      return CalculateTrampleDamage(p);
    }

    public static int CalculateTrampleDamage(AttackerEvaluationParameters p)
    {
      if (p.Attacker.Has().Trample == false)
        return 0;

      var total = p.Attacker.CalculateCombatDamageAmount(singleDamageStep: false, powerIncrease: p.AttackerPowerIncrease);
      total = total - p.Blockers.Sum(x => x.Card.Life + x.ToughnessIncrease);

      return total > 0 ? total : 0;
    }

    public static int CalculateDefendingPlayerLifeloss(AttackerEvaluationParameters p)
    {
      var total = 0;
            
      if (p.Blockers.None())
      {
        total = p.Attacker.CalculateCombatDamageAmount(singleDamageStep: false, powerIncrease: p.AttackerPowerIncrease);
      }
      else if (p.Attacker.Has().Trample)
      {
        total = CalculateTrampleDamage(p);
      }

      if (total == 0)
        return 0;

      var prevented = p.Attacker.Controller.Opponent.CalculatePreventedReceivedDamageAmount(total, p.Attacker,
        isCombat: true);

      return total - prevented;
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

      if (toughnessIncrease < 1 && !attacker.Has().FirstStrike)
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