namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class CombatEnchantment : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsAbleToAttack)
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
  }
}