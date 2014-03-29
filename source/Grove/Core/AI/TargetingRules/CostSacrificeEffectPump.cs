namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeEffectPump : TargetingRule
  {
    private readonly int _power;
    private readonly int _toughness;    

    private CostSacrificeEffectPump() {}

    public CostSacrificeEffectPump(int power, int toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var effectCanidates = p.Candidates<Card>(ControlledBy.SpellOwner, selector: c => c.Effect);

      if (IsAfterOpponentDeclaresBlockers(p.Controller))
      {
        effectCanidates = GetBestAttackersForPTGain(_power, _toughness, effectCanidates);
      }

      else if (IsAfterYouDeclareBlockers(p.Controller))
      {
        effectCanidates = GetBestBlockersForPTGain(_power, _toughness, effectCanidates);
      }

      var costCandidates = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Cost)
        .OrderBy(x => x.Score)
        .Take(1);

      return Group(costCandidates, effectCanidates,
        add1: (t, trgs) => trgs.AddCost(t),
        add2: (t, trgs) => trgs.AddEffect(t));
    }
  }
}