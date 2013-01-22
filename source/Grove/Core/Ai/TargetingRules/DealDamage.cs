namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class DealDamage : TargetingRule
  {
    public int? Amount;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (p.EffectTargetTypeCount > 1)
      {
        // e.g shower of sparks
        return SelectTargets2Selectors(p);
      }

      var candidates = GetCandidatesByDescendingDamageScore(p).ToList();
      return Group(candidates, p.MinTargetCount());
    }

    private IEnumerable<Targets> SelectTargets2Selectors(TargetingRuleParameters p)
    {
      if (p.EffectTargetTypeCount > 2)
        throw new NotSupportedException("More than 2 effect selectors not supported.");

      var candidates1 = GetCandidatesByDescendingDamageScore(p, selectorIndex: 0).ToList();
      var candidates2 = GetCandidatesByDescendingDamageScore(p, selectorIndex: 1).ToList();

      return Group(candidates1, candidates2);
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      // triggered abilities force you to choose a target even if its
      // not favorable e.g Flaming Kavu                      

      var candidates = p.Candidates<Card>().OrderByDescending(x => x.Toughness);
      return Group(candidates, p.MinTargetCount());
    }

    private IEnumerable<ITarget> GetCandidatesByDescendingDamageScore(TargetingRuleParameters p, int selectorIndex = 0)
    {
      var amount = Amount ?? p.MaxX;

      var candidates = p.Candidates<Player>(selectorIndex: selectorIndex)
        .Where(x => x == p.Controller.Opponent)
        .Select(x => new
          {
            Target = (ITarget) x,
            Score = ScoreCalculator.CalculateLifelossScore(x.Life, amount)
          })
        .Concat(
          p.Candidates<Card>(ControlledBy.Opponent)
            .Select(x => new
              {
                Target = (ITarget) x,
                Score = x.Life <= amount ? x.Score : 0
              }))
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target);

      return candidates;
    }
  }
}