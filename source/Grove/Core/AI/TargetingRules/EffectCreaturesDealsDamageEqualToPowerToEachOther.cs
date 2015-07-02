namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectCreaturesDealsDamageEqualToPowerToEachOther : TargetingRule
  {
    private readonly int _powerIncrement;
    private readonly int _toughnessIncrement;

    private EffectCreaturesDealsDamageEqualToPowerToEachOther() {}

    public EffectCreaturesDealsDamageEqualToPowerToEachOther(int powerIncrement, int toughnessIncrement)
    {
      _powerIncrement = powerIncrement;
      _toughnessIncrement = toughnessIncrement;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var yourCandidate = p.Candidates<Card>(selectorIndex: 0)
        .OrderByDescending(x => x.Power)
        .ThenByDescending(x => x.Toughness)
        .FirstOrDefault();

      if (yourCandidate == null)
        return Enumerable.Empty<Targets>();

      var opponentCandidates = p.Candidates<Card>(selectorIndex: 1)
        .Where(x =>
          x.Power < (yourCandidate.Toughness + _toughnessIncrement) &&
            x.Life <= (yourCandidate.Power + _powerIncrement))
        .OrderByDescending(x => x.Score)
        .ToList();

      return Group(new[] {yourCandidate}, opponentCandidates,
        (t, tgs) => tgs.AddEffect(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}