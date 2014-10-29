namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectDealDifferentDamage : TargetingRule
  {
    private readonly List<int> _amounts = new List<int>();


    public EffectDealDifferentDamage(IEnumerable<int> amounts)
    {
      _amounts.AddRange(amounts);
    }

    private EffectDealDifferentDamage() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var results = new List<Result>();

      var opponent = p.Candidates<Player>(ControlledBy.Opponent)
        .FirstOrDefault();

      if (opponent != null)
      {
        for (var i = 0; i < _amounts.Count; i++)
        {
          results.Add(new Result
            {
              Damage = _amounts[i],
              Score = ScoreCalculator.CalculateLifelossScore(opponent.Life, _amounts[i]),
              Target = opponent
            });
        }
      }

      var creatures = p.Candidates<Card>(ControlledBy.Opponent).ToArray();

      for (var i = 0; i < _amounts.Count; i++)
      {
        foreach (var creature in creatures.Where(x => x.Life <= _amounts[i]))
        {
          results.Add(new Result
            {
              Damage = _amounts[i],
              Score = creature.Score,
              Target = creature
            });
        }
      }

      var targets = results
        .GroupBy(x => x.Target)
        .Select(x => x.OrderBy(y => y.Damage).First())
        .OrderByDescending(x => x.Score)
        .ToArray();

      var picks = new List<ITarget>();
      for (var i = 0; i < _amounts.Count; i++)
      {
        var pick = targets
          .FirstOrDefault(x => x.Damage <= _amounts[i] && !picks.Contains(x.Target));

        if (pick == null)
        {
          return None<Targets>();
        }

        picks.Add(pick.Target);
      }

      return Group(picks, p.MinTargetCount());
    }

    private class Result
    {
      public int Damage;
      public int Score;
      public ITarget Target;
    }
  }
}