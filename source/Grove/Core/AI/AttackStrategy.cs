namespace Grove.AI
{
  using System.Collections.Generic;
  using System.Linq;
  using CombatRules;

  public class AttackStrategy
  {
    private readonly AttackStrategyParameters _p;

    public AttackStrategy(AttackStrategyParameters p)
    {
      _p = p;
    }

    private List<BlockAssignment> AssignEveryBlockerToEachAttacker()
    {
      return _p.AttackerCandidates
        .Select(attacker => new BlockAssignment(
          attacker,
          _p.BlockerCandidates
            .Where(attacker.CanBeBlockedBy)
            .ToList(),
          _p.DefendingPlayersLife))
        .ToList();
    }

    public List<Card> ChooseAttackers()
    {
      var assignments = AssignEveryBlockerToEachAttacker();

      return (from blockAssignment in assignments
              where blockAssignment.Score > 0
              select blockAssignment.Attacker).ToList();
    }

    private class BlockAssignment
    {
      public readonly Card Attacker;
      public readonly int Score;

      public BlockAssignment(Card attacker, List<Card> blockers, int defendersLife)
      {
        Attacker = attacker;
        Score = CalculateScore(new EvaluationParameters(attacker, blockers, defendersLife));
      }       

      private static BlockerEvaluationParameters GetBlockerParameters(CardWithCombatAbilities atttacker,
        CardWithCombatAbilities blocker)
      {
        return new BlockerEvaluationParameters
          {
            Attacker = atttacker.Card,
            Blocker = blocker.Card,
            AttackerPowerIncrease = atttacker.Abilities.PowerIncrease,
            AttackerToughnessIncrease = atttacker.Abilities.ToughnessIncrease,
            BlockerPowerIncrease = blocker.Abilities.PowerIncrease,
            BlockerToughnessIncrease = blocker.Abilities.ToughnessIncrease
          };
      }

      private static AttackerEvaluationParameters GetAttackerParamers(CardWithCombatAbilities attacker,
        IEnumerable<CardWithCombatAbilities> blockers)
      {
        var p = new AttackerEvaluationParameters(attacker.Card, attacker.Abilities.PowerIncrease,
          attacker.Abilities.ToughnessIncrease);

        foreach (var blocker in blockers)
        {
          p.AddBlocker(blocker.Card, blocker.Abilities.PowerIncrease,
            blocker.Abilities.ToughnessIncrease);
        }

        return p;
      }


      private int CalculateAttackerScore(EvaluationParameters p)
      {
        if (p.Attacker.Card.Has().Deathtouch)
          return 0;

        var attackerParamers = GetAttackerParamers(p.Attacker, p.Blockers);

        if (QuickCombat.CanAttackerBeDealtLeathalDamage(attackerParamers) && !p.Attacker.Abilities.CanRegenerate)
          return p.Attacker.Card.Score;

        return 0;
      }

      private int CalculateBlockersScore(EvaluationParameters p)
      {
        if (p.Blockers.Count == 0)
          return 0;

        var blockersRanks = p.Blockers.Select(blocker =>
          {
            var blockerParameters = GetBlockerParameters(p.Attacker, blocker);
            var attackerParameters = GetAttackerParamers(p.Attacker, new[] {blocker});

            return new
              {
                Blocker = blocker,
                CanBeKilled = QuickCombat.CanBlockerBeDealtLeathalCombatDamage(blockerParameters),
                CanKill = QuickCombat.CanAttackerBeDealtLeathalDamage(attackerParameters)
              };
          })
          .ToList();

        if (blockersRanks.Any(x => !x.CanBeKilled))
          return 0;

        var leastValuedBlockerWhoCanKill = blockersRanks.Where(x => x.CanKill)
          .OrderBy(x => x.Blocker.Card.Score)
          .FirstOrDefault();

        if (leastValuedBlockerWhoCanKill != null)
        {
          return leastValuedBlockerWhoCanKill.Blocker.Card.Score;
        }

        return blockersRanks.OrderBy(x => x.Blocker.Card.Score).First().Blocker.Card.Score;
      }

      private int CalculateScore(EvaluationParameters p)
      {
        return CalculateBlockersScore(p)
          + CalculateLifelossScore(p)
          - CalculateAttackerScore(p);
      }

      private int CalculateLifelossScore(EvaluationParameters p)
      {
        var attackerParameters = GetAttackerParamers(p.Attacker, p.Blockers);
        return ScoreCalculator.CalculateLifelossScore(p.DefendersLife,
          QuickCombat.CalculateDefendingPlayerLifeloss(attackerParameters));
      }

      private class CardWithCombatAbilities
      {
        public readonly CombatAbilities Abilities;
        public readonly Card Card;

        public CardWithCombatAbilities(Card card)
        {
          Card = card;
          Abilities = card.GetCombatAbilities();
        }
      }

      private class EvaluationParameters
      {
        public readonly CardWithCombatAbilities Attacker;
        public readonly List<CardWithCombatAbilities> Blockers;
        public readonly int DefendersLife;

        public EvaluationParameters(Card attacker, IEnumerable<Card> blockers, int defendersLife)
        {
          DefendersLife = defendersLife;
          Attacker = new CardWithCombatAbilities(attacker);
          Blockers = blockers.Select(x => new CardWithCombatAbilities(x)).ToList();
        }
      }
    }
  }
}