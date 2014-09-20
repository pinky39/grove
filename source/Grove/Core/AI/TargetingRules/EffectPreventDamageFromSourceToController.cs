namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPreventDamageFromSourceToController : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var targetPicks = new List<ITarget>();

      if (!Stack.IsEmpty && p.Candidates<ITarget>().Contains(Stack.TopSpell))
      {
        var damageToPlayer = Stack.GetDamageTopSpellWillDealToPlayer(p.Controller);

        if (damageToPlayer > 0)
        {
          targetPicks.Add(Stack.TopSpell);
        }
      }

      if (Turn.Step == Step.DeclareBlockers && Stack.IsEmpty)
      {
        if (!p.Controller.IsActive)
        {
          var attacker = Combat.FindAttackerWhichWillDealGreatestDamageToDefender(
            card => p.Candidates<ITarget>().Contains(card));

          if (attacker != null)
          {
            targetPicks.Add(attacker);
          }
        }
      }

      return Group(targetPicks, 1);
    }
  }
}