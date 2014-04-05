namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostTapEffectTap : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var costCandidates = p
        .Candidates<Card>(selector: c => c.Cost)
        .OrderBy(x => x.Score)
        .Take(1);

      var effectCandidates =
        p.Candidates<Card>(ControlledBy.Opponent, selector: c => c.Effect)
          .Where(x => !x.IsTapped)
          .OrderBy(x =>
            {
              if (x.Is().Creature)
                return 1;

              if (x.Is().Land)
                return 2;

              return 3;
            })
          .Take(1);          

      return Group(costCandidates, effectCandidates,
        add1: (t, trgs) => trgs.AddCost(t),
        add2: (t, trgs) => trgs.AddEffect(t));
    }
  }
}