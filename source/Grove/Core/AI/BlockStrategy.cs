namespace Grove.AI
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class BlockStrategy
  {
    public static ChosenBlockers ChooseBlockers(BlockStrategyParameters p)
    {
      // 1. Pass
      //
      // assign minimal number of  blockers to every attacker
      // keep assignments that have biggest positive gain            
      var possibleAssignments = new List<BlockerAssignment>();

      foreach (var attacker in p.Attackers)
      {
        var legalCandidates = p.BlockerCandidates.Where(blocker => attacker.CanBeBlockedBy(blocker)).ToList();
        var minimalAssignments = legalCandidates.KSubsets(attacker.MinimalBlockerCount);

        foreach (var blockers in minimalAssignments)
        {
          possibleAssignments.Add(new BlockerAssignment(attacker, blockers, p.DefendersLife));
        }
      }

      var assignments = new Dictionary<Card, BlockerAssignment>();
      var usedAttackers = new HashSet<Card>();

      foreach (var assignment in possibleAssignments.Where(x => x.Gain > 0).OrderByDescending(x => x.Gain))
      {
        if (assignment.Blockers.Any(assignments.ContainsKey))
          continue;

        if (usedAttackers.Contains(assignment.Attacker))
          continue;

        foreach (var blocker in assignment.Blockers)
        {
          assignments[blocker] = assignment;
        }

        usedAttackers.Add(assignment.Attacker);
      }

      var unassignedBlockers = p.BlockerCandidates
        .Where(x => !assignments.ContainsKey(x))
        .ToList();


      // 2. Pass
      // Assign additional blockers to blocked attackers 
      // if such an assignment would improove the gain
      return ImprooveAssignementsByAddingAditionalBlockers(assignments.Values.Distinct().ToList(), unassignedBlockers);
    }

    private static ChosenBlockers ImprooveAssignementsByAddingAditionalBlockers(
      List<BlockerAssignment> assignments, List<Card> unassignedBlockers)
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
      private bool _canAllInitialBlockersBeKilled;

      public BlockerAssignment(Card attacker, IEnumerable<Card> blockers, int defenderLife)
      {
        Attacker = attacker;
        AssignInitialBlockers(attacker, blockers, defenderLife);
      }

      public Card Attacker { get; private set; }

      public IEnumerable<Card> Blockers { get { return _blockers; } }

      public int Gain { get; private set; }
      public bool IsAttackerKilled { get; private set; }

      public void AssignAdditionalBlockers(List<Card> unassignedBlockers)
      {
        if (Attacker.Has().Deathtouch)
          return;

        var unassignedCopy = unassignedBlockers.ToList();

        foreach (var blocker in unassignedCopy)
        {
          if (Attacker.CanBeBlockedBy(blocker))
          {
            if (IsSafeBlock(blocker) || IncreasesGain(blocker))
            {
              unassignedBlockers.Remove(blocker);
              _blockers.Add(blocker);
            }
          }
        }
      }

      private void AssignInitialBlockers(Card attacker, IEnumerable<Card> blockers, int defendersLife)
      {
        _canAllInitialBlockersBeKilled = true;
        var attackerAbilities = attacker.GetCombatAbilities();

        var attackerEvaluationParameters = new AttackerEvaluationParameters(attacker,
          attackerAbilities.PowerIncrease, attackerAbilities.ToughnessIncrease);

        foreach (var blocker in blockers)
        {
          var blockerAbilities = blocker.GetCombatAbilities();

          _blockers.Add(blocker);

          attackerEvaluationParameters.AddBlocker(blocker, blockerAbilities.PowerIncrease,
            blockerAbilities.ToughnessIncrease);

          var canBlockerBeDealtLeathalCombatDamage =
            QuickCombat.CanBlockerBeDealtLeathalCombatDamage(new BlockerEvaluationParameters
              {
                Attacker = attacker,
                Blocker = blocker,
                BlockerPowerIncrease = blockerAbilities.PowerIncrease,
                BlockerToughnessIncrease = blockerAbilities.ToughnessIncrease,
                AttackerPowerIncrease = attackerAbilities.PowerIncrease,
                AttackerToughnessIncrease = attackerAbilities.ToughnessIncrease
              });

          var blockerScore = canBlockerBeDealtLeathalCombatDamage && !blockerAbilities.CanRegenerate
            ? blocker.Score
            : 0;

          var lifelossScore = ScoreCalculator.CalculateLifelossScore(
            defendersLife,
            attacker.CalculateCombatDamageAmount(singleDamageStep: false));

          var trampleScore = ScoreCalculator.CalculateLifelossScore(
            defendersLife, QuickCombat.CalculateTrampleDamage(Attacker, blocker));

          Gain = lifelossScore - blockerScore - trampleScore;
          _canAllInitialBlockersBeKilled = _canAllInitialBlockersBeKilled && canBlockerBeDealtLeathalCombatDamage;
        }

        IsAttackerKilled = QuickCombat.CanAttackerBeDealtLeathalDamage(attackerEvaluationParameters);

        var attackerScore = IsAttackerKilled && !attackerAbilities.CanRegenerate
          ? attacker.Score
          : 0;

        Gain += attackerScore;
      }

      private bool IncreasesGain(Card additionalBlocker)
      {
        // attacker was not killed, but blocker was
        // check if additional blocker changes things

        if (_canAllInitialBlockersBeKilled == false)
          return false;                
        
        if (additionalBlocker.Score > Attacker.Score)
          return false;

        if (QuickCombat.CalculateTrampleDamage(Attacker, _blockers) > 0)
          return true;

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