namespace Grove.Artifical.TimingRules
{
  using System;
  using System.Linq;
  using Gameplay;
  using Gameplay.States;

  [Serializable]
  public class TargetRemoval : TimingRule
  {
    private readonly bool _combatOnly;

    private TargetRemoval() {}

    public TargetRemoval(bool combatOnly = false)
    {
      _combatOnly = combatOnly;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      // eot
      if (!p.Controller.IsActive && Turn.Step == Step.EndOfTurn && !_combatOnly)
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

      if (_combatOnly)
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