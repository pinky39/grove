namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeEffectPreventDamageFromSourceToTarget : EffectPreventDamageFromSourceToTarget
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var sourceTargetCandidates = GetDamageSourceAndDamageTargetCandidates(p);

      if (sourceTargetCandidates.DamageSource.Count == 0 ||
        sourceTargetCandidates.DamageTarget.Count == 0)
      {
        return None<Targets>();
      }

      // for simplicity we consider only 'best' 1 legal assignment
      var damageSource = sourceTargetCandidates.DamageSource[0];
      var damageTarget = sourceTargetCandidates.DamageTarget[0];

      // cost should not be paid with damage target
      var cost = p.Candidates<Card>(selector: trgs => trgs.Cost)
        .OrderBy(x => x.Score)
        .Where(x => x != damageTarget)
        .FirstOrDefault();

      if (cost == null)
      {
        return None<Targets>();
      }

      var targets = new Targets();
      targets.AddCost(cost);
      targets.AddEffect(damageSource);
      targets.AddEffect(damageTarget);

      return new[] {targets};
    }
  }
}