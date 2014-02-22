namespace Grove.Gameplay.AI.TargetingRules
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

      var candidates = new List<Card>();

      if (IsAfterOpponentDeclaresBlockers(p.Controller))
      {
        candidates.AddRange(GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, p));
      }

      else if (IsAfterYouDeclareBlockers(p.Controller))
      {
        candidates.AddRange(GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, p));
      }

      else if (toughness > 0)
      {
        candidates.AddRange(p.Candidates<Card>(ControlledBy.SpellOwner)
          .Where(x => Stack.CanBeDealtLeathalDamageByTopSpell(x.Card())));
      }

      if ((_untilEot == false) &&
        (IsEndOfOpponentsTurn(p.Controller) ||
          p.Card.IsPermanent && Stack.CanBeDestroyedByTopSpell(p.Card)))
      {
        candidates.AddRange(p.Candidates<Card>(ControlledBy.SpellOwner)
          .OrderBy(x => x.Toughness));
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}