namespace Grove.Artifical.TimingRules
{
  public class RegenerateTimingRule : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Card.Has().Indestructible)
        return false;

      if (Stack.CanBeDestroyedByTopSpell(p.Card))
        return true;

      return Combat.CanBeDealtLeathalCombatDamage(p.Card);
    }
  }
}