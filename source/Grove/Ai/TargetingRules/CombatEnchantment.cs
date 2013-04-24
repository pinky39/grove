namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Player;
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
            Score = CalculateAttackerScore(x)
          })
        .OrderByDescending(x => x.Score)
        .Where(x => x.Score > 0)
        .Select(x => x.Card);

      return Group(candidates, 1);
    }
  }
}