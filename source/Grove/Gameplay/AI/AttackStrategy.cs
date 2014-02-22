namespace Grove.Gameplay.AI
{
  using System;
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
      var results = new List<BlockAssignment>();
      
      var attackers = _p.AttackerCandidates.Select(card => 
        new CardWithCombatAbilities {
          Card = card,
          Abilities = card.GetCombatAbilities()
        }).ToList();

      var blockers = _p.BlockerCandidates.Select(card => 
        new CardWithCombatAbilities {
          Card = card,
          Abilities = card.GetCombatAbilities()
        }).ToList();

      foreach (var attacker in attackers)
      {
        var assignment = new BlockAssignment(attacker, _p.DefendingPlayersLife);

        foreach (var blocker in blockers)
        {
          assignment.TryAssignBlocker(blocker);
        }

        results.Add(assignment);
      }

      return results;
    }

    public List<Card> ChooseAttackers()
    {
      var assignments = AssignEveryBlockerToEachAttacker();

      return (from blockAssignment in assignments
              where blockAssignment.Score > 0
              select blockAssignment.Attacker.Card).ToList();
    }

    private class BlockAssignment
    {
      private readonly List<CardWithCombatAbilities> _blockers = new List<CardWithCombatAbilities>();
      private readonly int _defendersLife;
      private readonly Lazy<int> _score;


      public BlockAssignment(CardWithCombatAbilities attacker, int defendersLife)
      {
        Attacker = attacker;
        _defendersLife = defendersLife;
        _score = new Lazy<int>(CalculateScore);
      }

      public CardWithCombatAbilities Attacker { get; private set; }

      private int AttackerScore
      {
        get
        {
          if (Attacker.Card.Has().Deathtouch)
            return 0;

          if (CanAttackerBeKilled && !Attacker.Abilities.CanRegenerate)
            return Attacker.Card.Score;

          return 0;
        }
      }

      private int BlockersScore
      {
        get
        {
          if (_blockers.Count == 0)
            return 0;

          var maxAttackerDamage = Attacker.Card.CalculateCombatDamageAmount(
            singleDamageStep: false, powerIncrease: Attacker.Abilities.PowerIncrease);

          var score = 0;
          var blockers = _blockers.OrderByDescending(x =>
            {
              if (x.Abilities.CanRegenerate)
                return 0;

              return x.Card.Score;
            });

          foreach (var blocker in blockers)
          {
            var p = new BlockerEvaluationParameters
              {
                Attacker = Attacker.Card,
                Blocker = blocker.Card,
                AttackerPowerIncrease = Attacker.Abilities.PowerIncrease,
                AttackerToughnessIncrease = Attacker.Abilities.ToughnessIncrease,
                BlockerPowerIncrease = blocker.Abilities.PowerIncrease,
                BlockerToughnessIncrease = blocker.Abilities.ToughnessIncrease
              };

            if (QuickCombat.CanBlockerBeDealtLeathalCombatDamage(p))
            {
              maxAttackerDamage -= Attacker.Card.Has().Deathtouch ? 1 : blocker.Card.Life;
              
              if (!blocker.Abilities.CanRegenerate)
                score += blocker.Card.Score;
            }

            if (maxAttackerDamage <= 0)
              break;
          }

          return score;
        }
      }

      private bool CanAttackerBeKilled { get { return QuickCombat.CanAttackerBeDealtLeathalDamage(GetAttackerEvaluationParameters()); } }

      private int DefendersLifeloss { get { return QuickCombat.CalculateDefendingPlayerLifeloss(GetAttackerEvaluationParameters()); } }

      public int Score { get { return _score.Value; } }

      private AttackerEvaluationParameters GetAttackerEvaluationParameters()
      {
        var p = new AttackerEvaluationParameters(Attacker.Card, Attacker.Abilities.PowerIncrease,
          Attacker.Abilities.ToughnessIncrease);

        foreach (var blocker in _blockers)
        {
          p.AddBlocker(blocker.Card, blocker.Abilities.PowerIncrease,
            blocker.Abilities.ToughnessIncrease);
        }

        return p;
      }

      public void TryAssignBlocker(CardWithCombatAbilities blocker)
      {
        if (Attacker.Card.CanBeBlockedBy(blocker.Card))
          _blockers.Add(blocker);
      }

      private int CalculateScore()
      {
        return BlockersScore + LifelossScore(_defendersLife) - AttackerScore;
      }

      private int LifelossScore(int defendersLife)
      {
        return ScoreCalculator.CalculateLifelossScore(defendersLife, DefendersLifeloss);
      }
    }

    private class CardWithCombatAbilities
    {
      public CombatAbilities Abilities;
      public Card Card;
    }
  }
}