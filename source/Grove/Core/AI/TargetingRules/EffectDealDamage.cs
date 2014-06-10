namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class EffectDealDamage : TargetingRule
  {
    private readonly Func<TargetingRuleParameters, int> _getAmount;

    private EffectDealDamage() {}

    public EffectDealDamage(Func<TargetingRuleParameters, int> getAmount)
    {
      _getAmount = getAmount;
    }

    public EffectDealDamage(int? amount = null) : this(p => amount ?? p.MaxX) {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (p.DistributeAmount > 0)
      {
        return SelectTargetsDistribute(p);
      }

      if (p.EffectTargetTypeCount > 1)
      {
        // e.g shower of sparks
        return SelectTargets2Selectors(p);
      }

      var candidates = GetCandidatesByDescendingDamageScore(p).ToList();
      return Group(candidates, p.MinTargetCount());
    }

    private IEnumerable<Targets> SelectTargetsDistribute(TargetingRuleParameters p)
    {
      var amount = p.DistributeAmount;

      // 0-1 knapsack optimization problem

      var targets = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => x.Is().Creature && x.Life <= amount)
        .Select(x => new KnapsackItem<ITarget>(
          item: x,
          weight: x.Life,
          value: x.Score))
        .ToList();

      targets.AddRange(
        p.Candidates<Player>(ControlledBy.Opponent)
        .SelectMany(x =>
          {
            var items = new List<KnapsackItem<ITarget>>();            
            var life = x.Player().Life;

            if (life <= amount)
            {
              // if killing blow is dealt add only one option
              
              items.Add(
                new KnapsackItem<ITarget>(
                  item: x,
                  weight: amount,
                  value: ScoreCalculator.CalculateLifelossScore(life, amount)));
            }
            else
            {
              const int decrementSoPlayerAtMostOnce = 1;

              // add each combination of damage
              // 1 damage, 2 damage, 3 damage ... amount damage
              for (var i = 1; i <= amount; i++)
              {
                items.Add(
                  new KnapsackItem<ITarget>(
                    item: x,
                    weight: i,
                    value: ScoreCalculator.CalculateLifelossScore(life, i) - decrementSoPlayerAtMostOnce));
              }
            }

            return items;
          })
        );

      if (targets.Count == 0)
        return None<Targets>();

      var solution = Knapsack.Solve(targets, amount);
      var selected = solution.Select(x => x.Item).ToList();

      var distribution = solution
        .Select(x => x.Weight)
        .ToList();

      return Group(selected, damageDistribution: distribution);
    }

    private IEnumerable<Targets> SelectTargets2Selectors(TargetingRuleParameters p)
    {
      Asrt.True(p.EffectTargetTypeCount <= 2, "More than 2 effect selectors currently not supported.");

      var candidates1 = GetCandidatesByDescendingDamageScore(p, selectorIndex: 0).ToList();
      var candidates2 = GetCandidatesByDescendingDamageScore(p, selectorIndex: 1).ToList();

      return Group(candidates1, candidates2);
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      // triggered abilities force you to choose a target even if its
      // not favorable e.g Flaming Kavu                      

      var candidates = p.Candidates<Card>().OrderByDescending(x => x.Toughness);
      return Group(candidates, p.MinTargetCount());
    }

    private IEnumerable<ITarget> GetCandidatesByDescendingDamageScore(TargetingRuleParameters p, int selectorIndex = 0)
    {
      var amount = _getAmount(p);      

      var candidates = p.Candidates<Player>(selectorIndex: selectorIndex)
        .Where(x => x == p.Controller.Opponent)
        .Select(x => new
          {
            Target = (ITarget) x,
            Score = ScoreCalculator.CalculateLifelossScore(x.Life, amount)
          })
        .Concat(
          p.Candidates<Card>(ControlledBy.Opponent)
            .Select(x => new
              {
                Target = (ITarget) x,
                Score = x.Life <= amount && x.CanBeDestroyed ? x.Score : 0
              }))
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target);

      return candidates;
    }
  }
}