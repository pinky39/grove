namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class ReduceToughness : TargetingRule
  {
    public int? Amount;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var amount = Amount ?? p.MaxX;

      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Target = x,
            Score = x.Life <= amount ? x.Score : 0
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target);

      return Group(candidates, p.MinTargetCount());
    }
  }
}