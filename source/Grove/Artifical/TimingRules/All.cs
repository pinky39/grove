namespace Grove.Artifical.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;

  public class All : TimingRule
  {
    private readonly List<TimingRule> _rules;

    private All() {}

    public All(params TimingRule[] rules)
    {
      _rules = rules.ToList();
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return _rules.All(x => x.ShouldPlay(p));
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