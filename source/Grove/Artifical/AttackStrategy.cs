namespace Grove.Artifical
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;

  public class AttackStrategy : IEnumerable<Card>
  {
    private readonly List<Card> _attackers;

    public AttackStrategy(
      int attackersLife,
      int defendersLife,
      IEnumerable<Card> attackerCandidates,
      IEnumerable<Card> blockerCandidates)
    {
      _attackers = ChooseAttackers(
        attackersLife,
        defendersLife,
        attackerCandidates.ToList(), blockerCandidates.ToList())
        .ToList();
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return _attackers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private static IEnumerable<BlockAssignment> AssignEveryBlockerToEachAttacker(
      IEnumerable<ScoredCard> attackerCandidates,
      IEnumerable<ScoredCard> blockerCandidates,
      int defendersLife)
    {
      var assignments = new List<BlockAssignment>();

      foreach (var attackerCandidate in attackerCandidates)
      {
        var assignment = new BlockAssignment(attackerCandidate, defendersLife);
        foreach (var blockerCandidate in blockerCandidates)
        {
          assignment.TryAssignBlocker(blockerCandidate);
        }

        assignments.Add(assignment);
      }
      return assignments;
    }

    private static IEnumerable<Card> ChooseAttackers(
      int attackersLife, int defendersLife, List<Card> attackerCandidates, List<Card> blockerCandidates)
    {
      var assignments = AssignEveryBlockerToEachAttacker(
        ScoreCards(attackerCandidates),
        ScoreCards(blockerCandidates),
        defendersLife);

      return (from blockAssignment in assignments
              where blockAssignment.Score > 0
              orderby blockAssignment.Score descending
              select blockAssignment.Attacker);
    }

    private static IEnumerable<ScoredCard> ScoreCards(IEnumerable<Card> cards)
    {
      return cards.Select(x => new ScoredCard
        {
          Card = x,
          Score = ScoreCalculator.CalculatePermanentScore(x)
        });
    }

    private class BlockAssignment
    {
      private readonly ScoredCard _attacker;
      private readonly List<ScoredCard> _blockers = new List<ScoredCard>();
      private readonly int _defendersLife;
      private readonly Lazy<int> _score;

      public BlockAssignment(ScoredCard attacker, int defendersLife)
      {
        _attacker = attacker;
        _defendersLife = defendersLife;
        _score = new Lazy<int>(CalculateScore);
      }

      public Card Attacker { get { return _attacker.Card; } }

      private int AttackerScore
      {
        get
        {
          if (Attacker.Has().Deathtouch)
            return 0;

          return CanAttackerBeKilled ? _attacker.Score : 0;
        }
      }

      private int BlockersScore
      {
        get
        {
          if (_blockers.Count == 0)
            return 0;

          var maxAttackerDamage = Attacker.CalculateCombatDamageAmount(singleDamageStep: false);
          var score = 0;

          foreach (var blocker in _blockers.Select(x => x.Card).OrderByDescending(x => x.Score))
          {
            if (QuickCombat.CanBlockerBeDealtLeathalCombatDamage(blocker, Attacker))
            {
              maxAttackerDamage -= Attacker.Has().Deathtouch ? 1 : blocker.Life;
              score += blocker.Score;
            }

            if (maxAttackerDamage <= 0)
              break;
          }

          return score;
        }
      }

      private bool CanAttackerBeKilled
      {
        get
        {
          return QuickCombat.CanAttackerBeDealtLeathalDamage(
            Attacker, _blockers.Select(x => x.Card));
        }
      }

      private int DefendersLifeloss
      {
        get
        {
          return QuickCombat.CalculateDefendingPlayerLifeloss(
            Attacker,
            _blockers.Select(x => x.Card)
            );
        }
      }

      public int Score { get { return _score.Value; } }

      public void TryAssignBlocker(ScoredCard scoredCard)
      {
        if (Attacker.CanBeBlockedBy(scoredCard.Card))
          _blockers.Add(scoredCard);
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

    private class ScoredCard
    {
      public Card Card { get; set; }
      public int Score { get; set; }
    }
  }
}