namespace Grove.AI.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class Any : TimingRule
  {
    private List<TimingRule> _cached;
    private readonly List<TimingRule> _rules;

    private Any() {}

    public Any(params TimingRule[] rules)
    {
      _rules = rules.ToList();
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      _cached = _rules        
        .Where(rule => !rule.ShouldPlayBeforeTargets(p))        
        .ToList();

      return _cached.Count < _rules.Count;
    }

    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      return _rules.Select(rule => !_cached.Contains(rule) && rule.ShouldPlayAfterTargets(p)).Any(x => x);                       
    }   

    public override void Initialize(Game game)
    {
      foreach (var rule in _rules)
      {
        rule.Initialize(game);
      }
    }
  }
}