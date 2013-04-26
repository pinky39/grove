namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card;
  using Gameplay.Targeting;

  public class SacrificeToRegenerate : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(x => x.Score);

      return Group(candidates, p.MinTargetCount(),
        add: (trg, trgs) => trgs.Cost.Add(trg));
    }
  }
}