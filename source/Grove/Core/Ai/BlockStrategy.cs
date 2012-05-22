namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using Controllers.Results;

  public class BlockStrategy
  {
    public BlockStrategy(IEnumerable<Card> attackers, IEnumerable<Card> blockerCandidates, int defendersLife)
    {
      Result = ChooseBlockers(attackers.ToList(), blockerCandidates.ToList(), defendersLife);
    }

    public ChosenBlockers Result { get; private set; }

    private static ChosenBlockers ChooseBlockers(List<Card> attackers, List<Card> blockerCandidates, int defendersLife)
    {
      // this is a gredy approach which does not always find optimal solution      
      // first pass, try assign every 1 blocker to every attacker
      // keep assignments that have biggest positive gain            
      var possibleAssignments = new List<BlockerAssignment>();

      foreach (var attacker in attackers)
      {
        foreach (var blockerCandidate in blockerCandidates)
        {
          possibleAssignments.Add(new BlockerAssignment(attacker, blockerCandidate, defendersLife));
        }
      }

      var assignments = new Dictionary<Card, BlockerAssignment>();
      var usedAttackers = new HashSet<Card>();      

      foreach (var assignment in possibleAssignments.OrderByDescending(x => x.Gain))
      {
        if (assignment.Gain <= 0)
        {
          break;
        }

        if (assignments.ContainsKey(assignment.FirstBlocker))
          continue;

        if (usedAttackers.Contains(assignment.Attacker))
          continue;

        assignments[assignment.FirstBlocker] = assignment;
        usedAttackers.Add(assignment.Attacker);
      }

      var unassignedBlockers = possibleAssignments
        .Where(x => !assignments.ContainsKey(x.FirstBlocker))
        .Select(x => x.FirstBlocker)
        .ToList();

      // second pass, assign additional blockers to blocked attackers 
      // if such an assignment would improove the gain
      return ImprooveAssignementsByAddingAditionalBlockers(assignments.Values, unassignedBlockers);
    }

    private static ChosenBlockers ImprooveAssignementsByAddingAditionalBlockers(
      IEnumerable<BlockerAssignment> assignments, List<Card> unassignedBlockers)
    {
      var orderedAssignments = assignments
        .Where(x => x.DamageNeededToKillAttacker > 0)
        .OrderBy(x => x.DamageNeededToKillAttacker)
        .ToList();

      foreach (var assignment in orderedAssignments)
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


    public static implicit operator ChosenBlockers(BlockStrategy strategy)
    {
      return strategy.Result;
    }

    private class BlockerAssignment
    {
      private readonly List<Card> _additionalBlockers = new List<Card>();
      private bool _isFirstBlockerKilled;

      public BlockerAssignment(Card attacker, Card blocker, int defenderLife)
      {
        Attacker = attacker;
        FirstBlocker = blocker;

        CalculateFirstBlockerGain(attacker, blocker, defenderLife);
      }

      public Card Attacker { get; private set; }

      public IEnumerable<Card> Blockers
      {
        get
        {
          yield return FirstBlocker;

          foreach (var blocker in _additionalBlockers)
          {
            yield return blocker;
          }
        }
      }

      public int DamageNeededToKillAttacker { get; private set; }
      public Card FirstBlocker { get; private set; }

      public int Gain { get; private set; }

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
            }
          }
        }
      }

      private void AssignAdditionalBlocker(Card blocker, List<Card> unassignedBlockers)
      {
        unassignedBlockers.Remove(blocker);
        _additionalBlockers.Add(blocker);
        DamageNeededToKillAttacker -= blocker.Power.Value;
      }

      private void CalculateFirstBlockerGain(Card attacker, Card blockerCandidate, int defendersLife)
      {
        if (!attacker.CanBeBlockedBy(blockerCandidate))
        {
          Gain = -1;
          return;
        }

        var blockerScore = attacker.CanDealLeathalDamageTo(blockerCandidate)
          ? ScoreCalculator.CalculatePermanentScore(blockerCandidate)
          : 0;

        _isFirstBlockerKilled = blockerScore > 0;


        var canKillAttacker = blockerCandidate.CanDealLeathalDamageTo(attacker);
        
        var attackerScore = canKillAttacker ? ScoreCalculator.CalculatePermanentScore(attacker): 0;        
        
        DamageNeededToKillAttacker = canKillAttacker ? 0 : attacker.LifepointsLeft - blockerCandidate.Power.Value;

        var lifelossScore = ScoreCalculator.CalculateLifelossScore(defendersLife, attacker.Power.Value);

        var trampleScore = Attacker.Has().Trample && attacker.Power.Value > blockerCandidate.LifepointsLeft
          ? ScoreCalculator.CalculateLifelossScore(defendersLife, attacker.Power.Value - blockerCandidate.LifepointsLeft)
          : 0;

        var scoreDefenderLoosesWhenBlocking = blockerScore - attackerScore + trampleScore;
        var scoreDefenderLoosesWhenNotBlocking = lifelossScore;

        Gain = scoreDefenderLoosesWhenNotBlocking - scoreDefenderLoosesWhenBlocking;
      }

      private bool IncreasesGain(Card additionalBlocker)
      {
        return _isFirstBlockerKilled &&
          DamageNeededToKillAttacker > 0 &&
            DamageNeededToKillAttacker <= additionalBlocker.Power.Value &&
              ScoreCalculator.CalculatePermanentScore(additionalBlocker) >
                ScoreCalculator.CalculatePermanentScore(Attacker);
      }

      private bool IsSafeBlock(Card additionalBlocker)
      {
        return !_isFirstBlockerKilled &&
          additionalBlocker.LifepointsLeft > Attacker.Power.Value;
      }
    }
  }
}