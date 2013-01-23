namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

  public class TargetRemoval : TimingRule
  {
    public bool CombatOnly;

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      // eot
      if (!p.Controller.IsActive && Turn.Step == Step.EndOfTurn && !CombatOnly)
        return true;

      // remove blockers
      if (p.Controller.IsActive && Turn.Step == Step.BeginningOfCombat)
      {
        return p.Targets<Card>().Any(x => x.CanBlock());
      }

      // remove attackers
      if (!p.Controller.IsActive && Turn.Step == Step.DeclareAttackers)
      {
        return p.Targets<Card>().Any(x => x.IsAttacker);
      }

      if (CombatOnly)
        return false;

      // play as response to some spells
      if (Stack.TopSpell != null && Stack.TopSpell.Controller == p.Controller.Opponent &&
        Stack.TopSpell.HasCategory(EffectCategories.Protector | EffectCategories.ToughnessIncrease))
      {
        if (Stack.TopSpell.Targets.Count > 0)
        {
          return Stack.TopSpell.Targets.Any(
            target => p.Targets<Card>().Any(x => x == target));
        }

        // e.g Nantuko Shade gives self a +1/+1 boost
        if (Stack.TopSpell.TargetsEffectSource)
        {
          return p.Targets<Card>().Any(x => x == Stack.TopSpell.Source.OwningCard);
        }
      }

      return false;
    }
  }
}