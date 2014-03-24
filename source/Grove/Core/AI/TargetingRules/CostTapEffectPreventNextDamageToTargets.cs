namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostTapEffectPreventNextDamageToTargets : TargetingRule
  {
    private readonly int _amount;

    private CostTapEffectPreventNextDamageToTargets() {}

    public CostTapEffectPreventNextDamageToTargets(int amount)
    {
      _amount = amount;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var effectCandidates = PreventNextDamage.GetCandidates(_amount, p, Game);

      if (effectCandidates.Count == 0)
        return None<Targets>();

      var costCandidates = p.Candidates<Card>()
        .OrderBy(x => x.Score)
        .Take(1).Cast<ITarget>()
        .ToList();

      return Group(costCandidates, effectCandidates,
        add1: (t, trgs) => trgs.AddCost(t),
        add2: (t, trgs) => trgs.AddEffect(t));
    }
  }
}