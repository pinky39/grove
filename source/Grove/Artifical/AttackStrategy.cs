namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;

  public class AttackStrategy
  {
    private readonly AttackStrategyParameters _p;

    public AttackStrategy(AttackStrategyParameters p)
    {
      _p = p;
    }

    private IEnumerable<BlockAssignment> AssignEveryBlockerToEachAttacker()
    {
      foreach (var attackerCandidate in _p.AttackerCandidates)
      {
        var assignment = new BlockAssignment(attackerCandidate, _p.DefendingPlayersLife);
        foreach (var blockerCandidate in _p.BlockerCandidates)
        {
          assignment.TryAssignBlocker(blockerCandidate);
        }

        yield return assignment;
      }
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
      private readonly List<Card> _blockers = new List<Card>();
      private readonly int _defendersLife;
      private readonly Lazy<int> _score;

      public BlockAssignment(Card attacker, int defendersLife)
      {
        Attacker = attacker;
        _defendersLife = defendersLife;
        _score = new Lazy<int>(CalculateScore);
      }

      public Card Attacker { get; private set; }

      private int AttackerScore
      {
        get
        {
          if (Attacker.Has().Deathtouch)
            return 0;

          return CanAttackerBeKilled ? Attacker.Score : 0;
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

          var blockers = _blockers.OrderByDescending(x => x.Toughness);

          foreach (var blocker in blockers)
          {
            if (QuickCombat.CanBlockerBeDealtLeathalCombatDamage(Attacker, blocker))
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
            Attacker, _blockers);
        }
      }

      private int DefendersLifeloss
      {
        get
        {
          return QuickCombat.CalculateDefendingPlayerLifeloss(
            Attacker, _blockers);
        }
      }

      public int Score { get { return _score.Value; } }

      public void TryAssignBlocker(Card card)
      {
        if (Attacker.CanBeBlockedBy(card))
          _blockers.Add(card);
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
  }
}