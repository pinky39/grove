namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.States;
  using Gameplay.Targeting;

  public class PreventDamageFromSourceToController : TargetingRule
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

      if (Turn.Step == Step.DeclareBlockers)
      {
        if (!p.Controller.IsActive)
        {
          var attacker = Combat.GetAttackerWhichWillDealGreatestDamageToDefender(
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