namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class CostSacrificeToReduceToughness : TargetingRule
  {
    private readonly int _amount;

    private CostSacrificeToReduceToughness() {}

    public CostSacrificeToReduceToughness(int amount)
    {
      _amount = amount;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var costCandidates = GetCandidatesThatCanBeDestroyed(p, s => s.Cost)
        .OrderBy(x => x.Score)
        .ToList();

      costCandidates.AddRange(
        p.Candidates<Card>(selector: s => s.Cost)
          .Where(x => !costCandidates.Contains(x))
          .OrderBy(x => x.Score));

      var effectCandidates = p.Candidates<Card>(ControlledBy.Opponent, selector: s => s.Effect)
        .Select(x => new
          {
            Target = x,
            Score = x.Life <= _amount ? x.Score : 0
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target)
        .ToList();

      return Group(costCandidates, effectCandidates,
        (t, tgs) => tgs.AddCost(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}