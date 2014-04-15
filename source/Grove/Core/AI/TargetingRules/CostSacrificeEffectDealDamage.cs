namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeEffectDealDamage : TargetingRule
  {
    private readonly int _amount;

    private CostSacrificeEffectDealDamage() {}

    public CostSacrificeEffectDealDamage(int amount)
    {
      _amount = amount;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      List<ITarget> costTargets;

      if (Stack.IsEmpty)
      {
        costTargets = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Cost)
          .OrderBy(x => x.Score)
          .Take(1)
          .Cast<ITarget>()
          .ToList();
      }
      else
      {
        costTargets = p.Candidates<Card>(selector: s => s.Cost)
          .Where(x => Stack.CanBeDestroyedByTopSpell(x.Card()))
          .OrderBy(x => x.Score)
          .Take(1)
          .Cast<ITarget>()
          .ToList();
      }

      var effectTargets = p.Candidates<Player>(ControlledBy.Opponent, selector: c => c.Effect)
        .Cast<ITarget>()
        .Concat(p.Candidates<Card>(ControlledBy.Opponent, selector: c => c.Effect)
          .Select(x => new
            {
              Target = x,
              Score = x.Life <= _amount && x.CanBeDestroyed ? x.Score : 0
            })
          .Where(x => x.Score > 0)
          .OrderByDescending(x => x.Score)
          .Select(x => x.Target))
        .ToList();

      return Group(costTargets, effectTargets,
        add1: (t, trgs) => trgs.AddCost(t),
        add2: (t, trgs) => trgs.AddEffect(t));
    }
  }
}