namespace Grove.Artifical
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Decisions.Results;
  using Infrastructure;

  public class BlockStrategy
  {
    private readonly BlockStrategyParameters _p;

    public BlockStrategy(BlockStrategyParameters p)
    {
      _p = p;
    }

    public ChosenBlockers ChooseBlockers()
    {
      // this is a gredy approach which does not always find optimal solution      
      // first pass, try assign every 1 blocker to every attacker
      // keep assignments that have biggest positive gain            
      var possibleAssignments = new List<BlockerAssignment>();

      foreach (var attacker in _p.Attackers)
      {
        foreach (var blockerCandidate in _p.BlockerCandidates)
        {
          if (attacker.CanBeBlockedBy(blockerCandidate))
          {
            possibleAssignments.Add(new BlockerAssignment(attacker, blockerCandidate, _p.DefendersLife));
          }
        }
      }

      var assignments = new Dictionary<Card, BlockerAssignment>();
      var usedAttackers = new HashSet<Card>();

      foreach (var assignment in possibleAssignments.Where(x => x.Gain > 0).OrderByDescending(x => x.Gain))
      {
        if (assignments.ContainsKey(assignment.Blockers.First()))
          continue;

        if (usedAttackers.Contains(assignment.Attacker))
          continue;

        assignments[assignment.Blockers.First()] = assignment;
        usedAttackers.Add(assignment.Attacker);
      }

      var unassignedBlockers = _p.BlockerCandidates
        .Where(x => !assignments.ContainsKey(x))
        .ToList();

      // second pass, assign additional blockers to blocked attackers 
      // if such an assignment would improove the gain
      return ImprooveAssignementsByAddingAditionalBlockers(assignments.Values, unassignedBlockers);
    }

    private static ChosenBlockers ImprooveAssignementsByAddingAditionalBlockers(
      IEnumerable<BlockerAssignment> assignments, List<Card> unassignedBlockers)
    {
      foreach (var assignment in assignments.Where(x => !x.IsAttackerKilled))
      {
        if (unassignedBlockers.Count == 0)
          break;

        assignment.AssignAdditionalBlockers(unassignedBlockers);
      }

      var result = new ChosenBlockers();

      foreach (var assignment in assignments)
      {
        foreach (var blocker in assignment.Blockers)
        {
          result.Add(blocker, assignment.Attacker);
        }
      }

      return result;
    }

    private class BlockerAssignment
    {
      private readonly List<Card> _blockers = new List<Card>();
      private bool _isAttackerKilled;
      private bool _isFirstBlockerKilled;

      public BlockerAssignment(Card attacker, Card firstBlocker, int defenderLife)
      {
        Attacker = attacker;
        _blockers.Add(firstBlocker);

        AssignFirstBlocker(attacker, firstBlocker, defenderLife);
      }

      public Card Attacker { get; private set; }

      public IEnumerable<Card> Blockers { get { return _blockers; } }

      public int Gain { get; private set; }

      public bool IsAttackerKilled { get { return _isAttackerKilled; } }

      public void AssignAdditionalBlockers(List<Card> unassignedBlockers)
      {
        if (Attacker.Has().Deathtouch)
          return;

        var unassignedCopy = unassignedBlockers.ToList();

        foreach (var blocker in unassignedCopy)
        {
          if (Attacker.CanBeBlockedBy(blocker))
          {
            if (IsSafeBlock(blocker))
            {
              AssignAdditionalBlocker(blocker, unassignedBlockers);
              continue;
            }

            if (IncreasesGain(blocker))
            {
              AssignAdditionalBlocker(blocker, unassignedBlockers);
              continue;
            }
          }
        }
      }

      private void AssignAdditionalBlocker(Card blocker, List<Card> unassignedBlockers)
      {
        unassignedBlockers.Remove(blocker);
        _blockers.Add(blocker);
      }

      private void AssignFirstBlocker(Card attacker, Card blocker, int defendersLife)
      {
        var blockerAbilities = blocker.GetCombatAbilities();
        var attackerAbilities = attacker.GetCombatAbilities();

        _isFirstBlockerKilled = QuickCombat.CanBlockerBeDealtLeathalCombatDamage(new BlockerEvaluationParameters
          {
            Attacker = attacker,
            Blocker = blocker,
            BlockerPowerIncrease = blockerAbilities.PowerIncrease,
            BlockerToughnessIncrease = blockerAbilities.ToughnessIncrease,
            AttackerPowerIncrease = attackerAbilities.PowerIncrease,
            AttackerToughnessIncrease = attackerAbilities.ToughnessIncrease
          });

        var blockerScore = _isFirstBlockerKilled && !blockerAbilities.CanRegenerate
          ? blocker.Score
          : 0;


        var attackerEvaluationParameters = new AttackerEvaluationParameters(attacker, blocker,
          attackerAbilities.PowerIncrease, attackerAbilities.ToughnessIncrease,
          blockerAbilities.PowerIncrease, blockerAbilities.ToughnessIncrease);

        _isAttackerKilled = QuickCombat.CanAttackerBeDealtLeathalDamage(attackerEvaluationParameters);

        var attackerScore = IsAttackerKilled && !attackerAbilities.CanRegenerate
          ? attacker.Score
          : 0;

        var lifelossScore = ScoreCalculator.CalculateLifelossScore(
          defendersLife,
          attacker.CalculateCombatDamageAmount(singleDamageStep: false));

        var trampleScore = ScoreCalculator.CalculateLifelossScore(
          defendersLife, QuickCombat.CalculateTrampleDamage(Attacker, blocker));

        var scoreDefenderLoosesWhenBlocking = blockerScore - attackerScore + trampleScore;
        var scoreDefenderLoosesWhenNotBlocking = lifelossScore;

        Gain = scoreDefenderLoosesWhenNotBlocking - scoreDefenderLoosesWhenBlocking;
      }

      private bool IncreasesGain(Card additionalBlocker)
      {
        // attacker was not killed, but blocker was
        // check if additional blocker changes things

        if (_isFirstBlockerKilled == false)
          return false;


        if (additionalBlocker.Score > Attacker.Score)
          return false;

        return QuickCombat.CanAttackerBeDealtLeathalDamage(
          Attacker,
          _blockers.Concat(additionalBlocker.ToEnumerable()));
      }

      private bool IsSafeBlock(Card additionalBlocker)
      {
        return !QuickCombat.CanBlockerBeDealtLeathalCombatDamage(Attacker, additionalBlocker);
      }
    }
  }
}