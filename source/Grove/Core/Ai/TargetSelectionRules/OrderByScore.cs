namespace Grove.Core.Ai.TargetSelectionRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class OrderByScore : TargetSelectionRule
  {
    public ControlledBy ControlledBy = ControlledBy.Opponent;
    public bool Descending = true;

    protected override IEnumerable<Targets> SelectTargets(TargetSelectionParameters p)
    {
      var orderedCandidates = p.Candidates<Card>(ControlledBy)
        .OrderByDescending(x => Descending ? x.Score : -x.Score)
        .Select(x => x.Card())
        .ToList();
      

      //if (p.IsTriggeredAbilityTarget && targetCount == 1 && orderedCandidates.Count == 0)
      //{
      //  return p.Targets(p.Candidates().OrderBy(x => x.Card().Score));
      //}

      //if (orderedCandidates.Count < targetCount)
      //  return p.NoTargets();

      //if (targetCount == 1)
      //{
      //  return p.Targets(p.Candidates(controlledBy));
      //}

      //var grouped = GroupCandidates(orderedCandidates, targetCount);
      //return p.MultipleTargets(grouped);
    }
  }
}