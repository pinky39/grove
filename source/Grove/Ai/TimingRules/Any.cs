namespace Grove.Ai.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay;

  public class Any : TimingRule
  {
    private readonly List<TimingRule> _rules;

    private Any() {}

    public Any(params TimingRule[] rules)
    {
      _rules = rules.ToList();
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return _rules.Any(x => x.ShouldPlay(p));
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