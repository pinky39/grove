namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class TapCreature : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => p.Controller.IsActive ? x.CanBlock() : x.CanAttack)
        .Select(x => new
          {
            Card = x,
            Damage = x.CalculateCombatDamage(allDamageSteps: true)
          })
        .Where(x => x.Damage > 0)
        .OrderByDescending(x => x.Damage)
        .ThenByDescending(x => x.Card.Score)
        .Select(x => x.Card);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}