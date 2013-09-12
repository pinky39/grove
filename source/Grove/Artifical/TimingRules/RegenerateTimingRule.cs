namespace Grove.Artifical.TimingRules
{
  public class RegenerateTimingRule : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      if (p.Card.Has().Indestructible)
        return false;

      if (Stack.CanBeDestroyedByTopSpell(p.Card))
        return true;

      return Stack.IsEmpty && Combat.CanBeDealtLeathalCombatDamage(p.Card);
    }
  }
}