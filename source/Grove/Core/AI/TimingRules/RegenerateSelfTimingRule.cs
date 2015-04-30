namespace Grove.AI.TimingRules
{
  public class RegenerateSelfTimingRule : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      if (p.Card.Has().Indestructible)
        return false;

      if (Stack.CanBeDestroyedByTopSpell(p.Card))
        return true;

      return Stack.IsEmpty && Turn.Step == Step.DeclareBlockers && Combat.CanBeDealtLeathalCombatDamage(p.Card);
    }
  }
}