namespace Grove.AI.TimingRules
{
  public class PumpTargetCardTimingRule : TimingRule
  {
    private readonly bool _untilEot;

    private PumpTargetCardTimingRule() {}

    public PumpTargetCardTimingRule(bool untilEot = true)
    {
      _untilEot = untilEot;
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      if (!Stack.IsEmpty)
      {
        return (Stack.TopSpell.HasTag(EffectTag.DealDamage) || 
          Stack.TopSpell.HasTag(EffectTag.ReduceToughness) || 
          Stack.TopSpell.HasTag(EffectTag.Destroy)) &&
          !Stack.TopSpell.HasTag(EffectTag.CannotRegenerate);
      }

      return IsBeforeYouDeclareAttackers(p.Controller) || IsAfterOpponentDeclaresBlockers(p.Controller) ||
        IsAfterYouDeclareBlockers(p.Controller) || (IsEndOfOpponentsTurn(p.Controller) && !_untilEot);
    }
  }
}