namespace Grove.Core.Ai.TimingRules
{
  public class OwningCardWillBeDestroyed : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step == Step.DeclareBlockers)
      {
        return Combat.CanBeDealtLeathalCombatDamage(p.Card);
      }

      if (Stack.IsEmpty)
        return false;

      return Stack.CanBeDestroyedByTopSpell(p.Card);
    }
  }
}