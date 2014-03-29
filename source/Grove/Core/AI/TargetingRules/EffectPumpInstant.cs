namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPumpInstant : TargetingRule
  {
    private readonly Func<TargetingRuleParameters, int?> _power;
    private readonly Func<TargetingRuleParameters, int?> _toughness;
    private readonly bool _untilEot;

    public EffectPumpInstant(Func<TargetingRuleParameters, int?> power,
      Func<TargetingRuleParameters, int?> toughness, bool untilEot = true)
    {
      _power = power;
      _toughness = toughness;
      _untilEot = untilEot;
    }

    public EffectPumpInstant(int? power, int? toughness, bool untilEot = true) : this(
      delegate { return power; }, delegate { return toughness; }, untilEot) {}

    private EffectPumpInstant() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = _power(p) ?? p.MaxX;
      var toughness = _toughness(p) ?? p.MaxX;

      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner);

      if (IsBeforeYouDeclareAttackers(p.Controller))
      {
        candidates = candidates.Select(x => new
          {
            Card = x.Card(),
            Score = CalculateAttackingPotential(x)
          })
          .OrderByDescending(x => x.Score)
          .Select(x => x.Card)
          .ToList();
      }

      else if (IsAfterOpponentDeclaresBlockers(p.Controller))
      {
        candidates = GetBestAttackersForPTGain(power, toughness, candidates);
      }

      else if (IsAfterYouDeclareBlockers(p.Controller))
      {
        candidates = GetBestBlockersForPTGain(power, toughness, candidates);
      }

      else if (_untilEot == false && IsEndOfOpponentsTurn(p.Controller))
      {
        candidates = candidates
          .OrderBy(x => x.Toughness);
      }

      else if (_untilEot == false && Stack.CanBeDestroyedByTopSpell(p.Card))
      {
        candidates = candidates
          .OrderBy(x => x.Toughness);
      }

      else if (toughness > 0)
      {
        candidates = candidates
          .Where(x => Stack.CanBeDealtLeathalDamageByTopSpell(x.Card()));
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}