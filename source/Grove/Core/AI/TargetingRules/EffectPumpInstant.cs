namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPumpInstant : TargetingRule
  {
    private readonly int? _power;
    private readonly int? _toughness;
    private readonly bool _untilEot;

    public EffectPumpInstant(int? power, int? toughness, bool untilEot = true)
    {
      _power = power;
      _untilEot = untilEot;
      _toughness = toughness;
    }

    private EffectPumpInstant() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = _power ?? p.MaxX;
      var toughness = _toughness ?? p.MaxX;

      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner);

      if (IsAfterOpponentDeclaresBlockers(p.Controller))
      {
        candidates = GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, candidates);
      }

      else if (IsAfterYouDeclareBlockers(p.Controller))
      {
        candidates = GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, candidates);
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