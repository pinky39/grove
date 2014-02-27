namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectCombatEnchantment : TargetingRule
  {
    private readonly Func<Card, bool> _filter;

    private EffectCombatEnchantment() {}

    public EffectCombatEnchantment(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsAbleToAttack)
        .Where(x => _filter(x))
        .Select(x => new
          {
            Card = x.Card(),
            Score = CalculateAttackingPotential(x)
          })
        .OrderByDescending(x => x.Score)
        .Where(x => x.Score > 0)
        .Select(x => x.Card);

      return Group(candidates, 1);
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => _filter(x))
        .Select(x => new
          {
            Card = x.Card(),
            Score = CalculateAttackingPotential(x)
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card)
        .ToList();

      if (candidates.Count == 0)
      {
        candidates.AddRange(
          p.Candidates<Card>(ControlledBy.Opponent)
            .OrderBy(x => x.Score));
      }

      return Group(candidates, 1);
    }
  }
}