namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;

  public class EffectPumpAttackerOrBlocker : TargetingRule
  {
    private readonly int? _power;
    private readonly int? _toughness;

    private EffectPumpAttackerOrBlocker() {}

    public EffectPumpAttackerOrBlocker(int? power, int? toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = _power ?? p.MaxX;
      var toughness = _toughness ?? p.MaxX;

      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner);;

      if (p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates = GetBestAttackersForPTGain(power, toughness, candidates);
      }
      else if (!p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates = GetBestBlockersForPTGain(power, toughness, candidates);
      }

      return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());
    }
  }
}