namespace Grove.AI.TimingRules
{
  public class RegenerateTargetTimingRule : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      if (Stack.IsEmpty)
      {
        return Turn.Step == Step.DeclareBlockers;
      }

      return (Stack.TopSpell.HasTag(EffectTag.DealDamage) || Stack.TopSpell.HasTag(EffectTag.Destroy)) && 
        !Stack.TopSpell.HasTag(EffectTag.CannotRegenerate);
    }
  }
}