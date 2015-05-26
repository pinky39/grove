namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectReduceToughness : TargetingRule
  {
    private readonly Func<TargetingRuleParameters, int> _getAmount;

    public EffectReduceToughness(int? amount = null) : this(p => amount ?? p.MaxX) {}

    public EffectReduceToughness(Func<TargetingRuleParameters, int> getAmount)
    {
      _getAmount = getAmount;
    }

    private EffectReduceToughness() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var amount = _getAmount(p);

      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Target = x,
            Score = x.Life <= amount ? 2 * x.Score : x.Score
          })        
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target);

      return Group(candidates, p.TotalMinTargetCount());
    }
  }
}