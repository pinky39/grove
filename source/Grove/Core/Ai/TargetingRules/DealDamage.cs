namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Targeting;

  public class DealDamage : TargetingRule
  {
    public int? Amount;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (p.DistributeDamage)
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
      var amount = Amount ?? p.MaxX;

      // 0-1 knapsack optimization problem

      var targets = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => x.Is().Creature && x.Life <= amount)
        .Select(x => new KnapsackItem<ITarget>(
          item: x,
          weight: x.Life,
          value: x.Score))
        .ToList();

      targets.AddRange(p.Candidates<Player>(ControlledBy.Opponent)
        .SelectMany(x =>
          {
            var items = new List<KnapsackItem<ITarget>>();

            const int decrementSoPlayerAtMostOnce = 1;

            for (var i = 1; i <= amount; i++)
            {
              items.Add(
                new KnapsackItem<ITarget>(
                  item: x,
                  weight: i,
                  value: ScoreCalculator.CalculateLifelossScore(x.Player().Life, i) - decrementSoPlayerAtMostOnce));
            }

            return items;
          })
        );

      if (targets.Count == 0)
        return None<Targets>();

      var solution = Knapsack.Solve(targets, amount);
      var selected = solution.Select(x => x.Item).ToList();

      var distribution = new DamageDistributor
        {
          Distribution = (t, a) => solution
            .Select(x => x.Weight)
            .ToList()
        };

      return Group(selected, damageDistributor: distribution);
    }

    private IEnumerable<Targets> SelectTargets2Selectors(TargetingRuleParameters p)
    {
      if (p.EffectTargetTypeCount > 2)
        throw new NotSupportedException("More than 2 effect selectors currently not supported.");

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
      var amount = Amount ?? p.MaxX;

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
                Score = x.Life <= amount ? x.Score : 0
              }))
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target);

      return candidates;
    }
  }
}