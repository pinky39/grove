namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Targeting;

  public class IncreasePowerOrToughness : TargetingRule
  {
    private readonly int? _power;
    private readonly int? _toughness;
    private readonly bool _untilEot;

    public IncreasePowerOrToughness(int? power, int? toughness, bool untilEot = true)
    {
      _power = power;
      _untilEot = untilEot;
      _toughness = toughness;
    }

    private IncreasePowerOrToughness() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = _power ?? p.MaxX;
      var toughness = _toughness ?? p.MaxX;

      var candidates = new List<Card>();

      if (p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates.AddRange(GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, p));
      }

      else if (!p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates.AddRange(GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, p));
      }

      if (toughness > 0)
      {
        candidates.AddRange(p.Candidates<Card>(ControlledBy.SpellOwner)
          .Where(x => Stack.CanBeDealtLeathalDamageByTopSpell(x.Card())));
      }

      if ((_untilEot == false) &&
        (p.Controller.IsActive == false && Turn.Step == Step.EndOfTurn ||
          p.Card.IsPermanent && Stack.CanBeDestroyedByTopSpell(p.Card)))
      {
        candidates.AddRange(p.Candidates<Card>(ControlledBy.SpellOwner)
          .OrderBy(x => x.Toughness));
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}