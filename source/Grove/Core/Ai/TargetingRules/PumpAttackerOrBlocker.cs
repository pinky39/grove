namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Targeting;

  public class PumpAttackerOrBlocker : TargetingRule
  {
    public int? Power;
    public int? Toughness;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = Power ?? p.MaxX;
      var toughness = Toughness ?? p.MaxX;

      if (p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        var candidates = GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, p);
        return Group(candidates, p.MinTargetCount());
      }

      if (!p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        var candidates = GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, p);
        return Group(candidates, p.MinTargetCount());
      }

      return None();
    }

    private IEnumerable<Card> GetCandidatesForAttackerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetingRuleParameters p)
    {
      return p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsAttacker)
        .Select(
          x =>
            new
              {
                Card = x,
                Gain =
                  QuickCombat.CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
                    attacker: x,
                    blockers: Combat.GetBlockers(x),
                    powerIncrease: powerIncrease.Value,
                    toughnessIncrease: toughnessIncrease.Value)
              })
        .Where(x => x.Gain > 0)
        .OrderByDescending(x => x.Gain)
        .Select(x => x.Card);
    }

    private IEnumerable<Card> GetCandidatesForBlockerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetingRuleParameters p)
    {
      return p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsBlocker)
        .Select(
          x =>
            new
              {
                Card = x,
                Gain =
                  QuickCombat.CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
                    blocker: x,
                    attacker: Combat.GetAttacker(x),
                    powerIncrease: powerIncrease.Value,
                    toughnessIncrease: toughnessIncrease.Value)
              })
        .Where(x => x.Gain > 0)
        .OrderByDescending(x => x.Gain)
        .Select(x => x.Card);
    }
  }
}